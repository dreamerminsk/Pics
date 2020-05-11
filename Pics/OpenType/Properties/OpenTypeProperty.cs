using System.Collections.Generic;

namespace Pics.OpenType.Properties
{
    public class OpenTypeProperty : OpenTypeItem
    {
        public long Position { get; set; }
        public long Size { get; set; }
        public string Title { get; set; }

        public OpenTypeItem Parent { get; set; }

        public List<OpenTypeItem> Items { get; set; }

        public OpenTypeProperty(OpenTypeItem parent)
        {
            Parent = parent;
        }
    }
}
