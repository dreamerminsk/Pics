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
                return new VInt(firstByte & 0x7FL);
            }
            else if (firstByte > 63)
            {
                return new VInt((firstByte & 63L) << 8 + stream.ReadByte());
            }
            else if (firstByte > 31)
            {
                return new VInt((firstByte & 31L) << 16 + stream.ReadByte() << 8 + stream.ReadByte());
            }
            else if (firstByte > 15)
            {
                return new VInt((firstByte & 15L) << 24 + stream.ReadByte() << 16 + stream.ReadByte() << 8 + stream.ReadByte());
            }
            else
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
