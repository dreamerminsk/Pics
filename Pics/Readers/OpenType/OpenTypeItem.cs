using System.Collections.Generic;

namespace Pics.Readers.OpenType
{

    public interface OpenTypeItem
    {

        OpenTypeItem Parent { get; }

        List<OpenTypeItem> Items { get; }

    }

}
