using Pics.OpenType.IO;
using System.Collections.Generic;
using System.IO;

namespace Pics.OpenType
{
    public class OpenTypeFile : OpenTypeItem
    {

        private FileInfo fileInfo;

        private BinaryReader reader;

        private OffsetTable offsetTable;

        private Headers headers;

        private Tables tables;

        public OpenTypeFile(string fileName)
        {
            fileInfo = new FileInfo(fileName);
            if (fileInfo.Exists)
            {
                reader = new ByteOrderSwappingBinaryReader(fileInfo.OpenRead());
                offsetTable = OffsetTable.ReadFrom(reader);
                headers = Headers.ReadFrom(reader, offsetTable.NumTables);
                tables = new Tables(headers, reader);
            }
        }

        public long Position { get => 0; }
        public long Size { get => reader.BaseStream.Length; }
        public string Title { get => fileInfo.Name; }
        OpenTypeItem OpenTypeItem.Parent { get => null; }
        List<OpenTypeItem> OpenTypeItem.Items { get => OpenItems(); }

        public List<OpenTypeItem> OpenItems()
        {
            var items = new List<OpenTypeItem>
            {
                offsetTable
            };
            return items;
        }

    }
}
