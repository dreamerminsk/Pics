using System.IO;

namespace Pics.Readers
{
    public class EBMLConstants
    {
        public static long EBMLID = 0x1A45DFA3;
    }

    public class VInt
    {
        private long value;

        public VInt(long value)
        {
            this.Value = value;
        }

        public long Value { get => value; set => this.value = value; }

        public void WriteTo(Stream stream)
        {

        }

        public static VInt ReadFrom(Stream stream)
        {
            var firstByte = stream.ReadByte();
            if (firstByte > 127)
            {
                return new VInt((long)firstByte & 0x1FL);
            }

            return new VInt(0);
        }

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
