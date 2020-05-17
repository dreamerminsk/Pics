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

        private async void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private async void ProcessNextPage()
        {
            var torrents = await NnmClub.GetTorrents(page++);
            flowLayoutPanel1.Controls.Clear();
            torrents.ForEach(t =>
            {
                var view = new TorrentInfoView(t);
                flowLayoutPanel1.Controls.Add(view);
                UpdateTorrentStats(t);
                UpdateLikeStats(t);
            });
            treeView1.BeginUpdate();
            treeView1.Nodes[0].Nodes.Clear();
            foreach (KeyValuePair<string, long> item in UserTorrents.OrderBy(key => -key.Value))
            {
                long likes = 0;
                UserLikes.TryGetValue(item.Key, out likes);
                var userNode = treeView1.Nodes[0].Nodes.Add(
                    item.Key + " / " + item.Value + ", " + likes + " /");
                userNode.Tag = item.Key;
            }
            treeView1.EndUpdate();
        }

        private void UpdateTorrentStats(TorrentInfo t)
        {
            if (!UserTorrents.ContainsKey(t.User))
            {
                UserTorrents.Add(t.User, 1);
            }
            else
            {
                long total = 0;
                UserTorrents.TryGetValue(t.User, out total);
                UserTorrents.Remove(t.User);
                UserTorrents.Add(t.User, ++total);
            }
        }

        private void UpdateLikeStats(TorrentInfo t)
        {
            if (!UserLikes.ContainsKey(t.User))
            {
                UserLikes.Add(t.User, t.Likes);
            }
            else
            {
                long total = 0;
                UserLikes.TryGetValue(t.User, out total);
                UserLikes.Remove(t.User);
                total += t.Likes;
                UserLikes.Add(t.User, total);
            }
        }

        public int page = 1;

        public Dictionary<string, long> UserTorrents { get; } = new Dictionary<string, long>();
        public Dictionary<string, long> UserLikes { get; } = new Dictionary<string, long>();

        private void toolStripContainer1_LeftToolStripPanel_Click(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ProcessNextPage();
        }
    }
}
