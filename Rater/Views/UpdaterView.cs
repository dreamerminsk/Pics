using System.Windows.Forms;

namespace Rater.Views
{
    public partial class UpdaterView : Form
    {
        public UpdaterView()
        {
            InitializeComponent();
        }

        private void UpdaterView_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }
    }
}
