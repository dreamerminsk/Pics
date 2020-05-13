using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
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
            //var credential = GoogleCredential.FromFile(@"c:\Users\User\FB\karoMed-59cbeddc2d66.json");
            //FirestoreDb db = FirestoreDb.Create("karomed-534ba");
            FirestoreDb db = FirestoreDb.Create();
            var n = 0;
            HtmlNodeCollection nodes = html.DocumentNode.SelectNodes("//div[@class='row']");
            nodes.ToList().ForEach(node =>
            {
                var clinic = node.SelectSingleNode(".//h5/a");
                richTextBox1.AppendText((++n).ToString() + ". " + clinic.InnerText + "\r\n");
                richTextBox1.AppendText("\t" + clinic.Attributes["href"].Value + "\r\n");
                var addr = node.SelectSingleNode(".//p/span");
                richTextBox1.AppendText("\t" + addr.InnerText + "\r\n");
            });
        }
    }
}