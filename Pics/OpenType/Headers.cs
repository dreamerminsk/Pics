using Pics.View;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Pics.OpenType
{
    public class Headers : IEnumerable<TableHeader>, IItemable
    {

        public long Position { get; set; } = 0;
        public long Size { get; set; } = 0;

        public List<TableHeader> Entries { get; } = new List<TableHeader>();

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
                headers.Entries.Add(TableHeader.ReadFrom(reader));
            }
            return headers;
        }

        public List<ListViewItem> Items()
        {
            var items = new List<ListViewItem>();

            foreach (TableHeader table in Entries)
            {
                var item = new ListViewItem(table.TableTag);
                item.SubItems.Add(table.Position.ToString());
                item.SubItems.Add(table.Size.ToString());
                //item.SubItems.Add(SfntVersion.ToString("X"));
                items.Add(item);
            }
            return items;
        }

        public IEnumerator<TableHeader> GetEnumerator()
        {
            return Entries.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Entries.GetEnumerator();
        }
    }
}
