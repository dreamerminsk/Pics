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

        public OpenTypeFile(string fileName)
        {
            fileInfo = new FileInfo(fileName);
            if (fileInfo.Exists)
            {
                reader = new BinaryReader(fileInfo.OpenRead());
            }
        }

        public List<ListViewItem> Items()
        {
            var items = new List<ListViewItem>();

            var item = new ListViewItem("Offset Table");
            item.SubItems.Add("0");
            item.SubItems.Add("12");
            items.Add(item);
            item = new ListViewItem("Table Record entries");
            item.SubItems.Add("12");
            item.SubItems.Add("160");
            items.Add(item);
            item = new ListViewItem("Table entries");
            item.SubItems.Add("182");
            item.SubItems.Add("160");
            items.Add(item);
            return items;
        }
    }
}
