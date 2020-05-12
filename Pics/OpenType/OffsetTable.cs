using System;
using System.Collections.Generic;
using System.Linq;

namespace Pics.OpenType
{
    public class OffsetTable : OpenTypeItem
    {
        private readonly OpenTypeItem mParent;

        public OffsetTable(OpenTypeItem parent)
        {
            this.mParent = parent;
        }

        public long Position { get; } = 0;
        public long Size { get; } = 12;

        public uint SfntVersion { get; set; }
        public ushort NumTables { get; set; }
        public ushort SearchRange { get; set; }
        public ushort EntrySelector { get; set; }
        public ushort RangeShift { get; set; }

        public string Title => GetType().Name + " : " + string.Join(", ", GetType().GetInterfaces().Select(x => x.Name));
        List<OpenTypeItem> OpenTypeItem.Items { get => new List<OpenTypeItem>(); }
        OpenTypeItem OpenTypeItem.Parent { get => mParent; }

    }
}
