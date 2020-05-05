using Pics.Readers.IO;
using Pics.View;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Pics.Readers.OpenType
{
    public class OpenTypeFile : IItemable
    {

        private FileInfo fileInfo;

        private BinaryReader reader;

        private OffsetTable offsetTable;

        private Headers tables;

        public OpenTypeFile(string fileName)
        {
            fileInfo = new FileInfo(fileName);
            if (fileInfo.Exists)
            {
                reader = new ByteOrderSwappingBinaryReader(fileInfo.OpenRead());
                offsetTable = OffsetTable.ReadFrom(reader);
                tables = Headers.ReadFrom(reader, offsetTable.NumTables);
            }
        }

        public List<ListViewItem> Items()
        {
            var items = new List<ListViewItem>();

            var item = new ListViewItem("Offset Table");
            item.Tag = offsetTable;
            item.SubItems.Add(offsetTable.Position.ToSize());
            item.SubItems.Add(offsetTable.Size.ToSize());
            items.Add(item);

            item = new ListViewItem("Table Record entries");
            item.Tag = tables;
            item.SubItems.Add(tables.Position.ToSize());
            item.SubItems.Add(tables.Size.ToSize());
            items.Add(item);

            item = new ListViewItem("Table entries");
            item.SubItems.Add(tables.Min(x => x.Offset).ToSize());
            item.SubItems.Add(tables.Sum(x => x.Length).ToSize());
            items.Add(item);
            return items;
        }
    }
}
