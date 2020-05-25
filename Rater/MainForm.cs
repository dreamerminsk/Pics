using LinqToDB;
using Rater.Models;
using Rater.Properties;
using Rater.Utils;
using Rater.Views;
using Rater.Workers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Windows.Forms;

namespace Rater
{

    public partial class MainForm : Form
    {

        private Updater updater = new Updater();

        private UpdaterView updaterView = new UpdaterView();

        public Filter Filter { get; set; } = new Filter();

        public List<TorrentInfo> Torrents { get; } = new List<TorrentInfo>();

        public ObservableConcurrentDictionary<string, Stats> UserInfos { get; } = new ObservableConcurrentDictionary<string, Stats>();
        public ObservableConcurrentDictionary<string, Stats> CatInfos { get; } = new ObservableConcurrentDictionary<string, Stats>();
        public ObservableConcurrentDictionary<MonthYear, Stats> MonthInfos { get; } = new ObservableConcurrentDictionary<MonthYear, Stats>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            splitContainer1.SplitterDistance = Settings.Default.TreeViewWidth;
            using (var db = new NnmContext())
            {
                (from t in db.Torrents
                 group t by t.Category into g
                 select new { Category = g.Key, Count = g.Count(), Likes = g.Sum(t => t.Likes) }
                    ).ToList().ForEach(c => { CatInfos.Add(c.Category, new Stats { Count = c.Count, Likes = c.Likes }); });
                (from t in db.Torrents
                 group t by t.User into g
                 select new { User = g.Key, Count = g.Count(), Likes = g.Sum(u => u.Likes) }
                    ).ToList().ForEach(c => { UserInfos.Add(c.User, new Stats { Count = c.Count, Likes = c.Likes }); });
            }
        }

        private void UpdateTree()
        {
            treeView1.BeginUpdate();
            var userIdx = 0;
            foreach (KeyValuePair<string, Stats> item in UserInfos.OrderBy(key => -key.Value.Likes))
            {
                var userNode = userIdx < GetUsersNode().Nodes.Count ? GetUsersNode().Nodes[userIdx++] : GetUsersNode().Nodes.Add(userIdx++.ToString());
                userNode.Text = item.Key + " — " + item.Value.ToShortString();
                userNode.ToolTipText = item.Key + "\r\n" + item.Value.ToString();
                userNode.Tag = item.Key;
            }
            GetUsersNode().Text = "Юзеры — " + UserInfos.Count();
            var catIdx = 0;
            foreach (KeyValuePair<string, Stats> item in CatInfos.OrderBy(key => key.Key))
            {
                var catNode = catIdx < GetCatsNode().Nodes.Count ? GetCatsNode().Nodes[catIdx++] : GetCatsNode().Nodes.Add(catIdx++.ToString());
                catNode.Text = item.Key + " — " + item.Value.ToShortString();
                catNode.ToolTipText = item.Key + "\r\n" + item.Value.ToString();
                catNode.Tag = item.Key;
            }
            GetCatsNode().Text = "Категории — " + CatInfos.Count();
            var monthIdx = 0;
            foreach (KeyValuePair<MonthYear, Stats> item in MonthInfos.OrderBy(key => key.Key.GetDate(1)).Reverse())
            {
                var monthNode = monthIdx < GetMonthsNode().Nodes.Count ? GetMonthsNode().Nodes[monthIdx++] : GetMonthsNode().Nodes.Add(monthIdx++.ToString());
                monthNode.Text = item.Key.ToString() + " — " + item.Value.ToShortString();
                monthNode.ToolTipText = item.Key.ToString() + "\r\n" + item.Value.ToString();
                monthNode.Tag = item.Key;
            }
            GetMonthsNode().Text = "Месяцы — " + MonthInfos.Count();
            treeView1.EndUpdate();
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

        private void treeView1_Click(object sender, EventArgs e)
        {


        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var node = treeView1.SelectedNode;

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            updaterView.Show();
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
