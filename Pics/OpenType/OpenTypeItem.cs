using System.Collections.Generic;

namespace Pics.OpenType
{

    public interface OpenTypeItem
    {
        
        long Position { get; set; }
        
        long Size { get; set; }
        
        string Title { get; set; }

        OpenTypeItem Parent { get; }

        List<OpenTypeItem> Items { get; }

    }

}
