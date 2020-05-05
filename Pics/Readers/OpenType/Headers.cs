using Pics.View;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Pics.Readers.OpenType
{
    public class Headers : IEnumerable<TableRecord>, IItemable
    {

        public long Position { get; set; } = 0;
        public long Size { get; set; } = 0;

        public List<TableRecord> Entries { get; } = new List<TableRecord>();

        public Headers()
        {

        }

        public static Headers ReadFrom(BinaryReader reader, int tableCount)
        {
            var headers = new Headers();
            headers.Position = reader.BaseStream.Position;
            headers.Size = 16 * tableCount;
            for (int i = 0; i < tableCount; i++)
            {
                headers.Entries.Add(TableRecord.ReadFrom(reader));
            }
            return headers;
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
