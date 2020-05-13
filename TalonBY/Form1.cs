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
                    listView1.Items.Add(clinic.Name);
                    var g = listView1.CreateGraphics();
                    var m = g.MeasureString(clinic.Name, listView1.Font);
                    if (listView1.Columns[0].Width < (m.Width + 10))
                    {
                        listView1.Columns[0].Width = (int)m.Width + 10;
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
    }
}