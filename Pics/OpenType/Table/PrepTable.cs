using Pics.View;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Pics.OpenType.Table
{
    public class PrepTable : TableEntry, IItemable
    {
        private TableHeader header;

        public PrepTable(TableHeader header) : base(header)
        {
            this.header = header;
        }

        public override string Name => "prep";

        public byte[] instructions { get; private set; }

        public List<ListViewItem> Items()
        {
            var items = new List<ListViewItem>();

            for (var i = 0; i < instructions.Length; i++)
            {
                var item = new ListViewItem("OpCode: " + instructions[i].ToString("X"));

                item.SubItems.Add((Header.Offset + i).ToSize());
                item.SubItems.Add(1L.ToSize());
                //item.SubItems.Add(Version.ToString());
                items.Add(item);
            }

            return items;
        }

        public override void ReadFrom(BinaryReader reader)
        {
            reader.BaseStream.Position = Header.Offset;
            instructions = reader.ReadBytes((int)Header.Length);
        }
    }
}
