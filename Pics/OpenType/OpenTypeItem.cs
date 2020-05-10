using System.Collections.Generic;

namespace Pics.OpenType
{

    public interface OpenTypeItem
    {

        OpenTypeItem Parent { get; }

        List<OpenTypeItem> Items { get; }

    }

}
