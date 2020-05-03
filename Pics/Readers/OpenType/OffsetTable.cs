using Pics.View;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Pics.Readers.OpenType
{
    public class OffsetTable : IItemable
    {
        private Int64 pos = 0;
        private UInt32 sfntVersion;
        private UInt16 numTables;
        private UInt16 searchRange;
        private UInt16 entrySelector;
        private UInt16 rangeShift;

        public OffsetTable()
        {

        }

        public List<ListViewItem> Items()
        {
            var items = new List<ListViewItem>();

            var item = new ListViewItem("sfntVersion");
            item.SubItems.Add(pos.ToString());
            item.SubItems.Add(sizeof(UInt32).ToString());
            items.Add(item);

            item = new ListViewItem("numTables");
            item.SubItems.Add((pos + sizeof(UInt32)).ToString());
            item.SubItems.Add(sizeof(UInt16).ToString());
            items.Add(item);
            item = new ListViewItem("searchRange");
            item.SubItems.Add("182");
            item.SubItems.Add("160");
            items.Add(item);

            item = new ListViewItem("entrySelector");
            item.SubItems.Add("182");
            item.SubItems.Add("160");
            items.Add(item);

            item = new ListViewItem("rangeShift");
            item.SubItems.Add("182");
            item.SubItems.Add("160");
            items.Add(item);
            return items;
        }
    }
}
