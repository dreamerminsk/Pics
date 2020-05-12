using HtmlAgilityPack;
using System;
using System.Linq;
using System.Windows.Forms;

namespace TalonBY
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_LoadAsync(object sender, EventArgs e)
        {
            HtmlWeb web = new HtmlWeb();
            var html = await web.LoadFromWebAsync("https://talon.by/policlinics");
            var n = 0;
            HtmlNodeCollection nodes = html.DocumentNode.SelectNodes("//div/*/h5/a");
            nodes.ToList().ForEach(x =>
            {                
                richTextBox1.AppendText((++n).ToString() + ". " + x.InnerText + "\r\n");
                richTextBox1.AppendText("\t" + x.Attributes["href"].Value + "\r\n");
            });
        }
    }
}