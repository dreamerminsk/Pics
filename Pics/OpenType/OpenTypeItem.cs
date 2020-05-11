using System.Collections.Generic;

namespace Pics.OpenType
{

    public interface OpenTypeItem
    {

        long Position { get; }

        long Size { get; }

        string Title { get; }

        OpenTypeItem Parent { get; }

        List<OpenTypeItem> Items { get; }

    }

}
