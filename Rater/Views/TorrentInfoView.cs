using Rater.Clients;
using System;
using System.Diagnostics.Contracts;
using System.Windows.Forms;

namespace Rater.Views
{
    public partial class TorrentInfoView : UserControl
    {
        public TorrentInfoView(TorrentInfo torrentInfo)
        {
            Tag = torrentInfo;
            InitializeComponent();
        }

        private void toolStripContainer1_ContentPanel_Load(object sender, EventArgs e)
        {
            TorrentInfo ti = (TorrentInfo)Tag;
            label1.Text = ti.Title;
            label2.Text = ti.User;
            label3.Text = $"{ti.Published.ToShortDateString()} {ti.Published.ToLongTimeString()}";
            label6.Text = "      " + ti.Likes;
        }

        public void UpdateContent(TorrentInfo torrentInfo)
        {
            Contract.Requires(torrentInfo != null);
            Tag = torrentInfo;
            label1.Text = torrentInfo.Title;
            label2.Text = torrentInfo.User;
            label3.Text = $"{torrentInfo.Published.ToShortDateString()} {torrentInfo.Published.ToLongTimeString()}";
            label6.Text = "      " + torrentInfo.Likes;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
