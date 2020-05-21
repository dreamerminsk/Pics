using LinqToDB;
using Rater.Clients;
using Rater.Models;
using Rater.Properties;
using Rater.Utils;
using Rater.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rater
{

    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Observable.Timer(TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(16))
                .ObserveOn(this)
                .Subscribe(async t => await ProcessNextPageAsync().ConfigureAwait(true));
            splitContainer1.SplitterDistance = Settings.Default.TreeViewWidth;
            using (var db = new NnmContext())
            {
                (from p in db.Categories
                 orderby p.Name ascending
                 select p).ToList().ForEach(c => { CatInfos.Add(c.Name, new Stats { Count = 0, Likes = 0 }); });
                (from p in db.Users
                 orderby p.Name ascending
                 select p).ToList().ForEach(u => { UserInfos.Add(u.Name, new Stats { Count = 0, Likes = 0 }); });
            }
        }

        private async Task ProcessNextPageAsync()
        {
            toolStripStatusLabel1.Text = DateTime.Now.ToLongTimeString();
            toolStripStatusLabel2.Text = "Loading page " + Page;
            var torrents = await NnmClub.GetTorrents(Page++).ConfigureAwait(true);
            Torrents.AddRange(torrents);
            var idx = 0;
            Torrents.ForEach(t => UpdateStats(t));
            Torrents.Where(t =>
            {
                var found = false;
                if (!string.IsNullOrEmpty(Filter.Category))
                {
                    found = t.Category.Equals(Filter.Category, StringComparison.InvariantCultureIgnoreCase);
                }
                if ((!found) && (!string.IsNullOrEmpty(Filter.Month)))
                {
                    var my = new MonthYear(t.Published);
                    found = my.ToString().Equals(Filter.Month, StringComparison.InvariantCultureIgnoreCase);
                }
                if ((!found) && (!string.IsNullOrEmpty(Filter.User)))
                {
                    found = t.User.Equals(Filter.User, StringComparison.InvariantCultureIgnoreCase);
                }
                return found;
            }).OrderByDescending(t => t.Likes).Take(32).ToList().ForEach(t =>
              {
                  if (idx < flowLayoutPanel1.Controls.Count)
                  {
                      TorrentInfoView view = (TorrentInfoView)flowLayoutPanel1.Controls[idx++];
                      view.UpdateContent(t);
                  }
                  else
                  {
                      var view = new TorrentInfoView(t);
                      view.Dock = DockStyle.None;
                      flowLayoutPanel1.Controls.Add(view);
                      ++idx;
                  }

              });
            treeView1.BeginUpdate();
            var userIdx = 0;
            foreach (KeyValuePair<string, Stats> item in UserInfos.OrderBy(key => -key.Value.Likes))
            {
                var userNode = userIdx < GetUsersNode().Nodes.Count ? GetUsersNode().Nodes[userIdx++] : GetUsersNode().Nodes.Add(userIdx++.ToString());
                userNode.Text = item.Key + " — " + item.Value.ToShortString();
                userNode.ToolTipText = item.Key + "\r\n" + item.Value.ToString();
                userNode.Tag = item.Key;
            }
            GetUsersNode().Text = "Юзеры — " + UserInfos.Count;
            var catIdx = 0;
            foreach (KeyValuePair<string, Stats> item in CatInfos.OrderBy(key => key.Key))
            {
                var catNode = catIdx < GetCatsNode().Nodes.Count ? GetCatsNode().Nodes[catIdx++] : GetCatsNode().Nodes.Add(catIdx++.ToString());
                catNode.Text = item.Key + " — " + item.Value.ToShortString();
                catNode.ToolTipText = item.Key + "\r\n" + item.Value.ToString();
                catNode.Tag = item.Key;
            }
            GetCatsNode().Text = "Категории — " + CatInfos.Count;
            var monthIdx = 0;
            foreach (KeyValuePair<MonthYear, Stats> item in MonthInfos.OrderBy(key => key.Key.GetDate(1)).Reverse())
            {
                var monthNode = monthIdx < GetMonthsNode().Nodes.Count ? GetMonthsNode().Nodes[monthIdx++] : GetMonthsNode().Nodes.Add(monthIdx++.ToString());
                monthNode.Text = item.Key.ToString() + " — " + item.Value.ToShortString();
                monthNode.ToolTipText = item.Key.ToString() + "\r\n" + item.Value.ToString();
                monthNode.Tag = item.Key;
            }
            GetMonthsNode().Text = "Месяцы — " + MonthInfos.Count;
            treeView1.EndUpdate();
        }

        private void UpdateStats(TorrentInfo t)
        {
            if (!UserInfos.ContainsKey(t.User))
            {
                UserInfos.Add(t.User, new Stats { Count = 1, Likes = t.Likes });
                using (var db = new NnmContext())
                {
                    db.Insert(new UserInfo { Name = t.User });
                }
            }
            else
            {
                var total = new Stats();
                UserInfos.TryGetValue(t.User, out total);
                UserInfos.Remove(t.User);
                UserInfos.Add(t.User, new Stats { Count = total.Count + 1, Likes = total.Likes + t.Likes });
            }
            if (!CatInfos.ContainsKey(t.Category))
            {
                CatInfos.Add(t.Category, new Stats { Count = 1, Likes = t.Likes });
                using (var db = new NnmContext())
                {
                    db.Insert(new CategoryInfo { Name = t.Category });
                }
            }
            else
            {
                var total = new Stats();
                CatInfos.TryGetValue(t.Category, out total);
                CatInfos.Remove(t.Category);
                CatInfos.Add(t.Category, new Stats { Count = total.Count + 1, Likes = total.Likes + t.Likes });
            }
            if (!MonthInfos.ContainsKey(new MonthYear(t.Published)))
            {
                MonthInfos.Add(new MonthYear(t.Published), new Stats { Count = 1, Likes = t.Likes });
            }
            else
            {
                var total = new Stats();
                MonthInfos.TryGetValue(new MonthYear(t.Published), out total);
                MonthInfos.Remove(new MonthYear(t.Published));
                MonthInfos.Add(new MonthYear(t.Published), new Stats { Count = total.Count + 1, Likes = total.Likes + t.Likes });
            }
        }

        public int Page { get; set; } = 1;

        public Filter Filter { get; set; } = new Filter();

        public List<TorrentInfo> Torrents { get; } = new List<TorrentInfo>();

        public Dictionary<string, Stats> UserInfos { get; } = new Dictionary<string, Stats>();
        public Dictionary<string, Stats> CatInfos { get; } = new Dictionary<string, Stats>();
        public Dictionary<MonthYear, Stats> MonthInfos { get; } = new Dictionary<MonthYear, Stats>();

        private TreeNode GetUsersNode()
        {
            for (int i = 0; i < treeView1.Nodes.Count; i++)
            {
                if (treeView1.Nodes[i].Text.StartsWith("Юзеры", StringComparison.InvariantCulture))
                {
                    return treeView1.Nodes[i];
                }
            }
            return null;
        }

        private TreeNode GetCatsNode()
        {
            for (int i = 0; i < treeView1.Nodes.Count; i++)
            {
                if (treeView1.Nodes[i].Text.StartsWith("Категории", StringComparison.InvariantCulture))
                {
                    return treeView1.Nodes[i];
                }
            }
            return null;
        }

        private TreeNode GetMonthsNode()
        {
            for (int i = 0; i < treeView1.Nodes.Count; i++)
            {
                if (treeView1.Nodes[i].Text.StartsWith("Месяцы", StringComparison.InvariantCulture))
                {
                    return treeView1.Nodes[i];
                }
            }
            return null;
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Settings.Default.TreeViewWidth = splitContainer1.SplitterDistance;
            Settings.Default.Save();
        }

        private void treeView1_Click(object sender, EventArgs e)
        {


        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var node = treeView1.SelectedNode;
            if (node.Text.StartsWith("Категории", StringComparison.InvariantCultureIgnoreCase))
            {
                Filter.Category = null;
            }
            else if (node.Text.StartsWith("Месяцы", StringComparison.InvariantCultureIgnoreCase))
            {
                Filter.Month = null;
            }
            else if (node.Text.StartsWith("Юзеры", StringComparison.InvariantCultureIgnoreCase))
            {
                Filter.User = null;
            }
            else if (node.FullPath.StartsWith("Категории", StringComparison.InvariantCultureIgnoreCase))
            {
                var title = node.FullPath.Split('\\').Last().Trim();
                Filter.Category = title.Split('—').First().Trim();
            }
            else if (node.FullPath.StartsWith("Месяцы", StringComparison.InvariantCultureIgnoreCase))
            {
                var title = node.FullPath.Split('\\').Last().Trim();
                Filter.Month = title.Split('—').First().Trim();
            }
            else if (node.FullPath.StartsWith("Юзеры", StringComparison.InvariantCultureIgnoreCase))
            {
                var title = node.FullPath.Split('\\').Last().Trim();
                Filter.User = title.Split('—').First().Trim();
            }

        }
    }

    public class Filter
    {
        public string Category { get; set; } = null;
        public string Month { get; set; } = null;
        public string User { get; set; } = null;
    }

    public class Stats
    {
        public long Count { get; set; }
        public long Likes { get; set; }

        public string ToShortString() => $"{Count}, {Likes}";

        override
        public string ToString() => $"торренты: {Count}\r\nлайки: {Likes}";
    }

}
