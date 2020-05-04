using Pics.Readers.IO;
using Pics.View;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Pics.Readers.OpenType
{
    public class OpenTypeFile : IItemable
    {

        private FileInfo fileInfo;

        private BinaryReader reader;

        private OffsetTable offsetTable;

        private Tables tables;

        public OpenTypeFile(string fileName)
        {
            fileInfo = new FileInfo(fileName);
            if (fileInfo.Exists)
            {
                reader = new ByteOrderSwappingBinaryReader(fileInfo.OpenRead());
                offsetTable = OffsetTable.ReadFrom(reader);
                tables = Tables.ReadFrom(reader, offsetTable.NumTables);
            }
        }

        public List<ListViewItem> Items()
        {
            var items = new List<ListViewItem>();

            var item = new ListViewItem("Offset Table");
            item.Tag = offsetTable;
            item.SubItems.Add(offsetTable.Position.ToString());
            item.SubItems.Add(offsetTable.Size.ToString());
            items.Add(item);

            item = new ListViewItem("Table Record entries");
            item.Tag = tables;
            item.SubItems.Add(tables.Position.ToString());
            item.SubItems.Add(tables.Size.ToString());
            items.Add(item);

            item = new ListViewItem("Table entries");
            item.SubItems.Add("182");
            item.SubItems.Add("160");
            items.Add(item);
            return items;
        }
    }
}
