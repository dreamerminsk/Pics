using Rater.Clients;
using Rater.Properties;
using Rater.Utils;
using Rater.Views;
using System;
using System.Collections.Generic;
using System.Linq;
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
            timer1.Start();
            splitContainer1.SplitterDistance = Settings.Default.TreeViewWidth;
        }

        private async void ProcessNextPage()
        {
            toolStripStatusLabel1.Text = DateTime.Now.ToShortTimeString();
            toolStripStatusLabel2.Text = "Loading page " + page;
            var torrents = await NnmClub.GetTorrents(page++).ConfigureAwait(true);
            var idx = 0;
            //flowLayoutPanel1.SuspendLayout();
            torrents.ForEach(t =>
            {
                if (idx < flowLayoutPanel1.Controls.Count)
                {
                    TorrentInfoView view = (TorrentInfoView)flowLayoutPanel1.Controls[idx++];
                    view.UpdateContent(t);
                }
                else
                {
                    var view = new TorrentInfoView(t);
                    flowLayoutPanel1.Controls.Add(view);
                    ++idx;
                }
                UpdateStats(t);
            });
            //flowLayoutPanel1.ResumeLayout();
            treeView1.BeginUpdate();
            var userIdx = 0;
            foreach (KeyValuePair<string, Stats> item in UserInfos.OrderBy(key => -key.Value.Likes))
            {
                var userNode = userIdx < GetUsersNode().Nodes.Count ? GetUsersNode().Nodes[userIdx++] : GetUsersNode().Nodes.Add(userIdx++.ToString());
                userNode.Text = item.Key + " / " + item.Value.ToShortString() + " /";
                userNode.ToolTipText = item.Key + "\r\n" + item.Value.ToString();
                userNode.Tag = item.Key;
            }
            GetUsersNode().Text = "Юзеры / " + UserInfos.Count + " /";
            var catIdx = 0;
            foreach (KeyValuePair<string, Stats> item in CatInfos.OrderBy(key => key.Key))
            {
                var catNode = catIdx < GetCatsNode().Nodes.Count ? GetCatsNode().Nodes[catIdx++] : GetCatsNode().Nodes.Add(catIdx++.ToString());
                catNode.Text = item.Key + " / " + item.Value.ToShortString() + " /";
                catNode.ToolTipText = item.Key + "\r\n" + item.Value.ToString();
                catNode.Tag = item.Key;
            }
            GetCatsNode().Text = "Категории / " + CatInfos.Count + " /";
            var monthIdx = 0;
            foreach (KeyValuePair<MonthYear, Stats> item in MonthInfos.OrderBy(key => key.Key.GetDate(1)).Reverse())
            {
                var monthNode = monthIdx < GetMonthsNode().Nodes.Count ? GetMonthsNode().Nodes[monthIdx++] : GetMonthsNode().Nodes.Add(monthIdx++.ToString());
                monthNode.Text = item.Key.ToString() + " / " + item.Value.ToShortString() + " /";
                monthNode.ToolTipText = item.Key.ToString() + "\r\n" + item.Value.ToString();
                monthNode.Tag = item.Key;
            }
            GetMonthsNode().Text = "Месяцы / " + MonthInfos.Count + " /";
            treeView1.EndUpdate();
        }

        private void UpdateStats(TorrentInfo t)
        {
            if (!UserInfos.ContainsKey(t.User))
            {
                UserInfos.Add(t.User, new Stats { Count = 1, Likes = t.Likes });
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

        public int page = 1;

        public Dictionary<string, Stats> UserInfos { get; } = new Dictionary<string, Stats>();
        public Dictionary<string, Stats> CatInfos { get; } = new Dictionary<string, Stats>();
        public Dictionary<MonthYear, Stats> MonthInfos { get; } = new Dictionary<MonthYear, Stats>();

        private void timer1_Tick(object sender, EventArgs e)
        {
            ProcessNextPage();
        }

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
