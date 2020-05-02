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

            return items;
        }
    }
}
