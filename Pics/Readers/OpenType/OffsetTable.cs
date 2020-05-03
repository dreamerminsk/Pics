using Pics.View;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Pics.Readers.OpenType
{
    public class OffsetTable : IItemable
    {
        private long pos = 0;

        public OffsetTable()
        {

        }

        public uint SfntVersion { get; set; }
        public ushort NumTables { get; set; }
        public ushort SearchRange { get; set; }
        public ushort EntrySelector { get; set; }
        public ushort RangeShift { get; set; }

        public static OffsetTable ReadFrom(BinaryReader reader)
        {
            var offsetTable = new OffsetTable();
            offsetTable.SfntVersion = reader.ReadUInt32();
            offsetTable.NumTables = reader.ReadUInt16();
            return offsetTable;
        }

        public List<ListViewItem> Items()
        {
            var items = new List<ListViewItem>();

            var item = new ListViewItem("sfntVersion");
            item.SubItems.Add(pos.ToString());
            item.SubItems.Add(sizeof(uint).ToString());
            items.Add(item);

            item = new ListViewItem("numTables");
            item.SubItems.Add((pos + sizeof(uint)).ToString());
            item.SubItems.Add(sizeof(ushort).ToString());
            items.Add(item);

            item = new ListViewItem("searchRange");
            item.SubItems.Add((pos + sizeof(uint) + sizeof(ushort)).ToString());
            item.SubItems.Add(sizeof(ushort).ToString());
            items.Add(item);

            item = new ListViewItem("entrySelector");
            item.SubItems.Add((pos + sizeof(uint) + 2 * sizeof(ushort)).ToString());
            item.SubItems.Add(sizeof(ushort).ToString());
            items.Add(item);

            item = new ListViewItem("rangeShift");
            item.SubItems.Add((pos + sizeof(uint) + 3 * sizeof(ushort)).ToString());
            item.SubItems.Add(sizeof(ushort).ToString());
            items.Add(item);
            return items;
        }
    }
}
