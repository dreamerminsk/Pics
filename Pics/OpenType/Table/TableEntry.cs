using System.IO;

namespace Pics.OpenType.Table
{
    public abstract class TableEntry
    {
        public TableEntry(TableHeader header)
        {
            this.Header = header;
        }
        public TableHeader Header { get; set; }
        public abstract void ReadFrom(BinaryReader reader);
        public abstract string Name { get; }
        public uint TableLength => this.Header.Length;


    }
}
