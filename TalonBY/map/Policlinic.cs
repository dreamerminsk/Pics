using Google.Cloud.Firestore;

namespace TalonBY.map
{
    [FirestoreData]
    public class Policlinic
    {
        [FirestoreProperty]
        public string Name { get; set; }

        [FirestoreProperty]
        public string Ref { get; set; }

    }
}
