using Pics.View;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Pics.Readers.OpenType
{
    public class OpenTypeFile : IItemable
    {
        public OpenTypeFile(string fileName)
        {

        }

        public List<ListViewItem> Items()
        {
            var items = new List<ListViewItem>();

            var item = new ListViewItem("Offset Table");
            item.SubItems.Add("0");
            item.SubItems.Add("12");
            item = new ListViewItem("Table Record entries");
            item.SubItems.Add("12");
            item.SubItems.Add("160");
            return items;
        }
    }
}
