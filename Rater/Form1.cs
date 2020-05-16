using Rater.Clients;
using Rater.Views;
using System;
using System.Collections.Generic;
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
            var torrents = await NnmClub.GetTorrents();
            torrents.ForEach(t =>
            {
                var view = new TorrentInfoView(t);
                flowLayoutPanel1.Controls.Add(view);
                if (!Users.Contains(t.User))
                {
                    Users.Add(t.User);
                    var userNode = treeView1.Nodes[0].Nodes.Add(t.User);
                    userNode.Tag = t.User;
                }
            });

        }

        public List<string> Users { get; set; } = new List<string>();

        private void toolStripContainer1_LeftToolStripPanel_Click(object sender, EventArgs e)
        {

        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
