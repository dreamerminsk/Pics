using Pics.OpenType.Table;
using Pics.View;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Pics.OpenType
{
    public class Tables : IItemable
    {
        private readonly BinaryReader reader;

        public long Position { get; set; } = 0;
        public long Size { get; set; } = 0;

        public Dictionary<long, TableHeader> TablesByOffset { get; set; } = new Dictionary<long, TableHeader>();

        public Dictionary<string, TableHeader> TablesByName { get; } = new Dictionary<string, TableHeader>();

        public Tables(Headers headers, BinaryReader reader)
        {
            this.reader = reader;
            headers.OrderBy(x => x.Offset).ToList().ForEach(x => TablesByOffset.Add(x.Offset, x));
            headers.OrderBy(x => x.TableTag).ToList().ForEach(x => TablesByName.Add(x.TableTag, x));
        }

        public List<ListViewItem> Items()
        {
            var items = new List<ListViewItem>();

            foreach (TableHeader table in TablesByOffset.Values)
            {
                var item = new ListViewItem(table.TableTag);
                item.SubItems.Add(table.Offset.ToSize());
                item.SubItems.Add(table.Length.ToSize());
                switch (table.TableTag)
                {
                    case "head":
                        item.Tag = Head(table); break;
                    case "fpgm":
                        item.Tag = Fpgm(table); break;
                    case "prep":
                        item.Tag = Prep(table); break;
                    default:
                        break;
                }
                items.Add(item);
            }
            return items;
        }

        public HeadTable Head(TableHeader header)
        {
            var table = new HeadTable(header);
            if (TablesByName.ContainsKey(table.Name))
            {
                table.ReadFrom(reader);
            }
            return table;
        }

        public FpgmTable Fpgm(TableHeader header)
        {
            var table = new FpgmTable(header);
            if (TablesByName.ContainsKey(table.Name))
            {
                table.ReadFrom(reader);
            }
            return table;
        }

        public PrepTable Prep(TableHeader header)
        {
            var table = new PrepTable(header);
            if (TablesByName.ContainsKey(table.Name))
            {
                table.ReadFrom(reader);
            }
            return table;
        }


    }
}
