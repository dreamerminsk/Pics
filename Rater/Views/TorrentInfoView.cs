using Rater.Clients;
using System;
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
            label1.Text = ((TorrentInfo)Tag).Title;
            label2.Text = ((TorrentInfo)Tag).User;
        }
    }
}
