using System.Collections.Generic;
using System.Windows.Forms;

namespace Pics.View
{
    interface IItemable
    {
        List<ListViewItem> Items();
    }
}
