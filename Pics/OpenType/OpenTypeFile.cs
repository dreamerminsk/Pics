using Pics.View;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Pics.OpenType
{
    public class OpenTypeFile : IItemable, OpenTypeItem
    {

        private FileInfo fileInfo;

        private BinaryReader reader;

        private OffsetTable offsetTable;

        private Headers headers;

        private Tables tables;

        public OpenTypeFile(string fileName)
        {
            fileInfo = new FileInfo(fileName);
            if (fileInfo.Exists)
            {
                reader = new ByteOrderSwappingBinaryReader(fileInfo.OpenRead());
                offsetTable = OffsetTable.ReadFrom(reader);
                headers = Headers.ReadFrom(reader, offsetTable.NumTables);
                tables = new Tables(headers, reader);
            }
        }

        public OpenTypeItem Parent { get => null; }
        List<OpenTypeItem> OpenTypeItem.Items { get => OpenItems(); }

        public List<OpenTypeItem> OpenItems()
        {
            var items = new List<OpenTypeItem>
            {
                offsetTable
            };
            return items;
        }

        public List<ListViewItem> Items()
        {
            var items = new List<ListViewItem>();

            var item = new ListViewItem("Offset Table");
            item.Tag = offsetTable;
            item.SubItems.Add(offsetTable.Position.ToSize());
            item.SubItems.Add(offsetTable.Size.ToSize());
            items.Add(item);

            item = new ListViewItem("Table Record entries")
            {
                Tag = headers
            };
            item.SubItems.Add(headers.Position.ToSize());
            item.SubItems.Add(headers.Size.ToSize());
            items.Add(item);

            item = new ListViewItem("Table entries")
            {
                Tag = tables
            };
            item.SubItems.Add(headers.Min(x => x.Offset).ToSize());
            item.SubItems.Add(headers.Sum(x => x.Length).ToSize());
            items.Add(item);
            return items;
        }
    }
}
