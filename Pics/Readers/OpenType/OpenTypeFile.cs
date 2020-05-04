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

        public OpenTypeFile(string fileName)
        {
            fileInfo = new FileInfo(fileName);
            if (fileInfo.Exists)
            {
                reader = new ByteOrderSwappingBinaryReader(fileInfo.OpenRead());
                offsetTable = OffsetTable.ReadFrom(reader);
            }
        }

        public List<ListViewItem> Items()
        {
            var items = new List<ListViewItem>();

            var item = new ListViewItem("Offset Table");
            item.Tag = offsetTable;
            item.SubItems.Add("0");
            item.SubItems.Add("12");
            items.Add(item);
            item = new ListViewItem("Table Record entries");
            item.SubItems.Add("12");
            item.SubItems.Add((16 * offsetTable.NumTables).ToString());
            items.Add(item);
            item = new ListViewItem("Table entries");
            item.SubItems.Add("182");
            item.SubItems.Add("160");
            items.Add(item);
            return items;
        }
    }
}
