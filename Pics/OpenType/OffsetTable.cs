using System.Collections.Generic;
using System.IO;

namespace Pics.OpenType
{
    public class OffsetTable : OpenTypeItem
    {
        public OffsetTable()
        {

        }

        public long Position { get; set; } = 0;
        public long Size { get; set; } = 12;

        public uint SfntVersion { get; set; }
        public ushort NumTables { get; set; }
        public ushort SearchRange { get; set; }
        public ushort EntrySelector { get; set; }
        public ushort RangeShift { get; set; }

        public OpenTypeItem Parent => null;

        public string Title { get => this.GetType().Name; }
        List<OpenTypeItem> OpenTypeItem.Items { get => new List<OpenTypeItem>(); }
        OpenTypeItem OpenTypeItem.Parent { get => null; }

        public static OffsetTable ReadFrom(BinaryReader reader)
        {
            var offsetTable = new OffsetTable();
            offsetTable.Position = reader.BaseStream.Position;
            offsetTable.SfntVersion = reader.ReadUInt32();
            offsetTable.NumTables = reader.ReadUInt16();
            offsetTable.SearchRange = reader.ReadUInt16();
            offsetTable.EntrySelector = reader.ReadUInt16();
            offsetTable.RangeShift = reader.ReadUInt16();
            return offsetTable;
        }

    }
}
