﻿using Pics.OpenType;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Pics
{
    public partial class Form1 : Form
    {

        private OpenTypeItem current = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void log(string message)
        {
            this.Invoke(new MethodInvoker(() => richTextBox1.AppendText(message)));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var lastFontName = Properties.Settings.Default.LastFont;
            setCurrentFile(lastFontName);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
            }
            else
            {
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var fileName = openFileDialog1.FileName;
                setCurrentFile(fileName);
            }
        }

        private void setCurrentFile(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return;
            }
            current = null;
            treeView1.Nodes.Clear();
            treeView1.Nodes.Add(new FileInfo(fileName).Name);
            var fs = new OpenTypeFile(fileName);
            setListViewContent(fs);
            Properties.Settings.Default.LastFont = fileName;
            Properties.Settings.Default.Save();
        }

        private void setUpListViewContent()
        {
            if (current.Parent != null)
            {
                setListViewContent(current.Parent);
            }

        }

        private void setListViewContent(OpenTypeItem content)
        {
            listView1.BeginUpdate();
            listView1.Items.Clear();
            if (current != null)
            {
                ListViewItem prevItem = new ListViewItem("..");
                prevItem.Tag = current;
                listView1.Items.Add(prevItem);
            }
            current = content;
            var children = content.Items.Select(x => ItemToView(x));
            listView1.Items.AddRange(children.ToArray());
            listView1.EndUpdate();

            ListViewItem ItemToView(OpenTypeItem x)
            {
                var y = new ListViewItem(x.Title);
                y.SubItems.Add((x.Position+0).ToSize());
                y.SubItems.Add(x.Size.ToSize());
                return y;
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            foreach (string file in files) setCurrentFile(file);
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 1)
            {
                var item = listView1.SelectedItems[0];
                if (item.Text.Equals(".."))
                {
                    setUpListViewContent();
                    return;
                }
                var tag = item.Tag;
                if (tag == null)
                {
                    return;
                }
                if (typeof(OpenTypeItem).IsAssignableFrom(tag.GetType()))
                {
                    setListViewContent((OpenTypeItem)tag);
                }
            }

        }

    }
}
