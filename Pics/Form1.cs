using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace Pics
{
    public partial class Form1 : Form
    {

        private DirectoryInfo current;

        private List<FileInfo> files = new List<FileInfo>();

        public Form1()
        {
            InitializeComponent();

            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
            backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            toolStripButton1.Enabled = true;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                try
                {
                    processFolder(drive.RootDirectory);
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                }
            }
        }

        private void processFolder(DirectoryInfo di)
        {
            try
            {
                current = di;
                var fs = di.EnumerateFiles();
                foreach (FileInfo f in fs)
                {
                    if (f.Name.ToUpper().EndsWith(".BMP"))
                    {
                        files.Add(f);
                        toolStripStatusLabel1.Text = files.Count.ToString();
                        processBmp(f);
                    }
                }
                var ds = di.EnumerateDirectories();
                foreach (DirectoryInfo d in ds)
                    processFolder(d);
            }
            catch (Exception e)
            {
                //MessageBox.Show(e.Message);
            }

        }

        private void processBmp(FileInfo fi)
        {
            log("\r\n\r\n\r\n" + fi.Name + "\t" + fi.Length.ToString() + "\r\n\r\n");
            FileStream fs = fi.OpenRead();
            try
            {
                string sign = Char.ConvertFromUtf32(fs.ReadByte()) + Char.ConvertFromUtf32(fs.ReadByte());
                log("Signature: " + sign + "\r\n");
                var fileSize = fs.ReadByte() + 256 * fs.ReadByte() + 256 * 256 * fs.ReadByte() + 256 * 256 * 256 * fs.ReadByte();
                log("FileSize: " + fileSize.ToString() + "\r\n");
                log("Reserved1: " + (fs.ReadByte() + 256 * fs.ReadByte()).ToString() + "\r\n");
                log("Reserved2: " + (fs.ReadByte() + 256 * fs.ReadByte()).ToString() + "\r\n");
                var fileOffset = fs.ReadByte() + 256 * fs.ReadByte() + 256 * 256 * fs.ReadByte() + 256 * 256 * 256 * fs.ReadByte();
                log("FileOffset: " + fileOffset.ToString() + "\r\n");

                var headerSize = fs.ReadByte() + 256 * fs.ReadByte() + 256 * 256 * fs.ReadByte() + 256 * 256 * 256 * fs.ReadByte();
                log("HeaderSize: " + headerSize.ToString() + "\r\n");
            }
            finally
            {
                fs.Close();
            }
        }

        private void log(string message)
        {
            this.Invoke(new MethodInvoker(() => richTextBox1.AppendText(message)));
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
            toolStripButton1.Enabled = false;
        }
    }
}
