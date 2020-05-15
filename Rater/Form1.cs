using HtmlAgilityPack;
using MongoDB.Driver;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
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
            HtmlWeb web = new HtmlWeb();
            var page = await web.LoadFromWebAsync("http://nnmclub.to/");
            var torrents = page.DocumentNode.SelectNodes(".//table[@class='pline']");
            torrents.Where(x => IsTorentBlock(x)).ToList().ForEach(x =>
              {
                  var title = x.SelectSingleNode(".//td[@class='pcatHead']");
                  if (title != null)
                  {                      
                      GroupBox groupBox = new GroupBox();
                      groupBox.Text = title.InnerText;
                      groupBox.Width = 500;
                      groupBox.Anchor = AnchorStyles.Left;
                      Label l = new Label();
                      l.Text = "LABEL";
                      l.Location = new Point(20, 20);
                      groupBox.Controls.Add(l);
                      flowLayoutPanel1.Controls.Add(groupBox);
                  }

              });
        }

        private bool IsTorentBlock(HtmlNode node)
        {
            return node.SelectNodes(".//a[starts-with(@href, 'magnet')]") != null;
        }
    }
}
