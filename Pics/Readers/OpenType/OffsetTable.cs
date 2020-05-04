using Pics.View;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Pics.Readers.OpenType
{
    public class OffsetTable : IItemable
    {
        public OffsetTable()
        {

        }

        public long Position { get; set; } = 0;
        public long Size { get; } = 12;

        public uint SfntVersion { get; set; }
        public ushort NumTables { get; set; }
        public ushort SearchRange { get; set; }
        public ushort EntrySelector { get; set; }
        public ushort RangeShift { get; set; }

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

        public List<ListViewItem> Items()
        {
            var items = new List<ListViewItem>();

            var item = new ListViewItem("sfntVersion");
            item.SubItems.Add(Position.ToString());
            item.SubItems.Add(4.ToString());
            item.SubItems.Add(SfntVersion.ToString("X"));
            items.Add(item);

            item = new ListViewItem("numTables");
            item.SubItems.Add((Position + 4).ToString());
            item.SubItems.Add(2.ToString());
            item.SubItems.Add(NumTables.ToString());
            items.Add(item);

            item = new ListViewItem("searchRange");
            item.SubItems.Add((Position + 6).ToString());
            item.SubItems.Add(2.ToString());
            item.SubItems.Add(SearchRange.ToString());
            items.Add(item);

            item = new ListViewItem("entrySelector");
            item.SubItems.Add((Position + 8).ToString());
            item.SubItems.Add(2.ToString());
            item.SubItems.Add(EntrySelector.ToString());
            items.Add(item);

            item = new ListViewItem("rangeShift");
            item.SubItems.Add((Position + 10).ToString());
            item.SubItems.Add(2.ToString());
            item.SubItems.Add(RangeShift.ToString());
            items.Add(item);
            return items;
        }
    }
}
