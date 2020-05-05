using Pics.View;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Pics.Readers.OpenType
{
    public class Tables : IItemable
    {
        public long Position { get; set; } = 0;
        public long Size { get; set; } = 0;



        public List<TableRecord> Entries { get; } = new List<TableRecord>();

        public Tables(Headers headers)
        {
            Entries.AddRange(headers.OrderBy(x => x.Offset).ToList());
        }

        public List<ListViewItem> Items()
        {
            var items = new List<ListViewItem>();

            foreach (TableRecord table in Entries)
            {
                var item = new ListViewItem(table.TableTag);
                item.SubItems.Add(table.Offset.ToSize());
                item.SubItems.Add(table.Length.ToSize());
                //item.SubItems.Add(SfntVersion.ToString("X"));
                items.Add(item);
            }
            return items;
        }
    }
}
