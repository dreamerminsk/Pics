using Pics.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Pics.Readers.OpenType
{
    public class Tables : IEnumerable<TableRecord>, IItemable
    {

        public long Position { get; set; } = 0;
        public long Size { get; set; } = 0;

        public List<TableRecord> Entries { get; } = new List<TableRecord>();

        public Tables()
        {

        }

        public static Tables ReadFrom(BinaryReader reader, int tableCount)
        {
            var tables = new Tables();
            tables.Position = reader.BaseStream.Position;
            tables.Size = 16 * tableCount;
            for (int i = 0; i < tableCount; i++)
            {
                tables.Entries.Add(TableRecord.ReadFrom(reader));
            }
            return tables;
        }

        public List<ListViewItem> Items()
        {
            var items = new List<ListViewItem>();

            foreach (TableRecord table in Entries)
            {
                var item = new ListViewItem(table.TableTag);
                item.SubItems.Add(table.Position.ToString());
                item.SubItems.Add(table.Size.ToString());
                //item.SubItems.Add(SfntVersion.ToString("X"));
                items.Add(item);
            }
            return items;
        }

        public IEnumerator<TableRecord> GetEnumerator()
        {
            return Entries.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Entries.GetEnumerator();
        }
    }
}
