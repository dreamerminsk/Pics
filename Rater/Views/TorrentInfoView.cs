using Rater.Models;
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
            SetContent((TorrentInfo)Tag);
        }

        public void UpdateContent(TorrentInfo torrentInfo)
        {
            Contract.Requires(torrentInfo != null);
            Tag = torrentInfo;
            SetContent((TorrentInfo)Tag);
        }

        private void SetContent(TorrentInfo ti)
        {
            label1.Text = ti.Title;
            label2.Text = ti.User;
            label3.Text = $"{ti.Published.ToShortDateString()} {ti.Published.ToLongTimeString()}";
            label6.Text = "      " + ti.Likes;
            richTextBox1.Text = ti.Text;
        }
    }
}
