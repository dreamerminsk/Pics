using Rater.Clients;
using Rater.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Rater
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private async void ProcessNextPage()
        {
            toolStripStatusLabel1.Text = DateTime.Now.ToShortTimeString();
            toolStripStatusLabel2.Text = "Loading page " + page;
            var torrents = await NnmClub.GetTorrents(page++);
            flowLayoutPanel1.Controls.Clear();
            torrents.ForEach(t =>
            {
                var view = new TorrentInfoView(t);
                flowLayoutPanel1.Controls.Add(view);
                UpdateStats(t);
            });
            treeView1.BeginUpdate();
            GetUsersNode().Nodes.Clear();
            foreach (KeyValuePair<string, Stats> item in UserInfos.OrderBy(key => -key.Value.Likes))
            {
                var userNode = GetUsersNode().Nodes.Add(
                    item.Key + " / " + item.Value.Count + ", " + item.Value.Likes + " /");
                userNode.ToolTipText = item.Key + "\r\nторренты: " + item.Value.Count + "\r\nлайки: " + item.Value.Likes;
                userNode.Tag = item.Key;
            }
            GetUsersNode().Text = "Юзеры / " + UserInfos.Count + " /";
            GetCatsNode().Nodes.Clear();
            foreach (KeyValuePair<string, Stats> item in CatInfos.OrderBy(key => -key.Value.Likes))
            {
                var userNode = GetCatsNode().Nodes.Add(
                    item.Key + " / " + item.Value.Count + ", " + item.Value.Likes + " /");
                userNode.ToolTipText = item.Key + "\r\nторренты: " + item.Value.Count + "\r\nлайки: " + item.Value.Likes;
                userNode.Tag = item.Key;
            }
            GetCatsNode().Text = "Категории / " + CatInfos.Count + " /";
            GetMonthsNode().Nodes.Clear();
            foreach (KeyValuePair<MonthYear, Stats> item in MonthInfos.OrderBy(key => key.Key.GetDate(1)).Reverse())
            {
                var userNode = GetMonthsNode().Nodes.Add(
                    item.Key.Year + "-" + item.Key.Month.ToString("00") + " / " + item.Value.Count + ", " + item.Value.Likes + " /");
                userNode.ToolTipText = item.Key.Year + "-" + item.Key.Month.ToString("00") + "\r\nторренты: " + item.Value.Count + "\r\nлайки: " + item.Value.Likes;
                userNode.Tag = item.Key;
            }
            GetMonthsNode().Text = "Месяцы / " + CatInfos.Count + " /";
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
            if (!MonthInfos.ContainsKey(new MonthYear(t.Published.Month, t.Published.Year)))
            {
                MonthInfos.Add(new MonthYear(t.Published.Month, t.Published.Year), new Stats { Count = 1, Likes = t.Likes });
            }
            else
            {
                var total = new Stats();
                MonthInfos.TryGetValue(new MonthYear(t.Published.Month, t.Published.Year), out total);
                MonthInfos.Remove(new MonthYear(t.Published.Month, t.Published.Year));
                MonthInfos.Add(new MonthYear(t.Published.Month, t.Published.Year), new Stats { Count = total.Count + 1, Likes = total.Likes + t.Likes });
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
                if (treeView1.Nodes[i].Text.StartsWith("Юзеры"))
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
                if (treeView1.Nodes[i].Text.StartsWith("Категории"))
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
                if (treeView1.Nodes[i].Text.StartsWith("Месяцы"))
                {
                    return treeView1.Nodes[i];
                }
            }
            return null;
        }
    }

    public class Stats
    {
        public long Count { get; set; }
        public long Likes { get; set; }
    }

}
