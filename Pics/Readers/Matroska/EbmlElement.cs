namespace Pics.Readers.Matroska
{
    public class EbmlElement
    {

        private long mId;
        private long mSize;

        public EbmlElement()
        {

        }

        public long ID { get => mId; set => mId = value; }
        public long Size { get => mSize; set => mSize = value; }
    }
}
