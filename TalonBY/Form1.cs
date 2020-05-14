using Google.Cloud.Firestore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TalonBY.map;

namespace TalonBY
{
    public partial class Form1 : Form
    {

        private readonly string ProjectID = "karomed-534ba";
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_LoadAsync(object sender, EventArgs e)
        {
            //var credential = GoogleCredential.FromFile(@"c:\Users\User\FB\karoMed-59cbeddc2d66.json");
            FirestoreDb db = FirestoreDb.Create(ProjectID);
            CollectionReference policlinics = db.Collection("policlinics");
            var query = await policlinics.GetSnapshotAsync();
            if (query.Documents.Count == 0)
            {
                await Insert();
            }
            else
            {
                query.Documents.ToList()
                    .Select(clinic => clinic.ConvertTo<Policlinic>()).ToList()
                    .ForEach(clinic =>
                {
                    listView1.BeginUpdate();
                    var item = listView1.Items.Add(clinic.Name);
                    item.SubItems.Add(clinic.City);
                    item.SubItems.Add(clinic.Address);
                    var g = listView1.CreateGraphics();
                    var m = g.MeasureString(clinic.Name, listView1.Font);
                    if (listView1.Columns[0].Width < (m.Width + 10))
                    {
                        listView1.Columns[0].Width = (int)m.Width + 10;
                    }
                    m = g.MeasureString(clinic.City, listView1.Font);
                    if (listView1.Columns[1].Width < (m.Width + 10))
                    {
                        listView1.Columns[1].Width = (int)m.Width + 10;
                    }
                    m = g.MeasureString(clinic.Address, listView1.Font);
                    if (listView1.Columns[2].Width < (m.Width + 10))
                    {
                        listView1.Columns[2].Width = (int)m.Width + 10;
                    }
                    g.Dispose();
                    listView1.EndUpdate();
                });
            }
        }

        private async Task Insert()
        {
            FirestoreDb db = FirestoreDb.Create(ProjectID);
            CollectionReference policlinics = db.Collection("policlinics");
            var clinics = await Clinics.GetPoliclinicsAsync();
            clinics.ForEach(async clinic =>
            {
                await policlinics.AddAsync(clinic);
            });
        }

        private async Task UpdateP()
        {
            FirestoreDb db = FirestoreDb.Create(ProjectID);
            var policlinics = db.Collection("policlinics");
            var query = await policlinics.GetSnapshotAsync();
            var p = query.Select(x => x.ConvertTo<Policlinic>())
                .Where(x => x.City == null).ToList().First();
            var p2 = await Clinics.GetPoliclinic(p);
            var q2 = await policlinics.WhereEqualTo("Name", p2.Name).GetSnapshotAsync();
            await q2.First().Reference.SetAsync(new
            {
                City = p2.City,
                Address = p2.Address
            }, SetOptions.MergeAll);
            MessageBox.Show(p.Name + "\r\n" + p2.City + "\r\n" + p2.Address);
        }

        private async void updateToolStripMenuItem_ClickAsync(object sender, EventArgs e)
        {
            await UpdateP();
        }
    }
}