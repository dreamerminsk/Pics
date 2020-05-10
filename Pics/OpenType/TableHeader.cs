using System.IO;

namespace Pics.OpenType
{
    public class TableHeader
    {

        public long Position { get; set; } = 0;
        public long Size { get; } = 16;
        public string TableTag { get; set; }
        public uint CheckSum { get; set; }
        public uint Offset { get; set; }
        public uint Length { get; set; }

        public static TableHeader ReadFrom(BinaryReader reader)
        {
            return new TableHeader
            {
                Position = reader.BaseStream.Position,
                TableTag = reader.ReadTag(),
                CheckSum = reader.ReadUInt32(),
                Offset = reader.ReadUInt32(),
                Length = reader.ReadUInt32()
            };
        }
    }
}
