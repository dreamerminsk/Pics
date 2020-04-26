using System.IO;

namespace Pics.Readers
{
    public class EBMLConstants
    {
        public static long EBMLID = 0x1A45DFA3;
    }

    public class EBMLElement
    {

        private long mId;
        private long mSize;
        public EBMLElement()
        {

        }

        public long ID { get => mId; set => mId = value; }
        public long Size { get => mSize; set => mSize = value; }
    }

    public class EBMLMaster : EBMLElement
    {

    }

    public class MkvFile
    {
        private FileInfo sourceFile;

        public MkvFile(FileInfo sourceFile)
        {
            this.sourceFile = sourceFile;
        }


    }
}
