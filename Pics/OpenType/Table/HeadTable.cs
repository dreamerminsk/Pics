using Pics.View;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Pics.Readers.OpenType.Table
{
    public class HeadTable : TableEntry, IItemable
    {
        public override string Name => "head";

        private short _indexToLocFormat;

        public HeadTable(TableHeader header) : base(header)
        {
        }

        public uint Version { get; private set; }
        public uint FontRevision { get; private set; }
        public uint CheckSumAdjustment { get; private set; }
        public uint MagicNumber { get; private set; }
        public ushort Flags { get; private set; }
        public ushort UnitsPerEm { get; private set; }
        public ulong Created { get; private set; }
        public ulong Modified { get; private set; }
        public short XMin { get; set; }
        public short YMin { get; set; }
        public short XMax { get; set; }
        public short YMax { get; set; }
        public ushort MacStyle { get; private set; }
        public ushort LowestRecPPEM { get; private set; }
        public short FontDirectionHint { get; private set; }
        public bool WideGlyphLocations => _indexToLocFormat > 0;
        public short GlyphDataFormat { get; private set; }

        public override void ReadFrom(BinaryReader reader)
        {
            reader.BaseStream.Position = Header.Offset;
            Version = reader.ReadUInt32();
            FontRevision = reader.ReadUInt32();
            CheckSumAdjustment = reader.ReadUInt32();
            MagicNumber = reader.ReadUInt32();
            //if (MagicNumber != 0x5F0F3CF5) throw new Exception("Invalid magic number!" + MagicNumber.ToString("x"));
            Flags = reader.ReadUInt16();
            UnitsPerEm = reader.ReadUInt16();
            Created = reader.ReadUInt64();
            Modified = reader.ReadUInt64();
            XMin = reader.ReadInt16();
            YMin = reader.ReadInt16();
            XMax = reader.ReadInt16();
            YMax = reader.ReadInt16();
            MacStyle = reader.ReadUInt16();
            LowestRecPPEM = reader.ReadUInt16();
            FontDirectionHint = reader.ReadInt16();
            _indexToLocFormat = reader.ReadInt16();
            GlyphDataFormat = reader.ReadInt16();
        }

        List<ListViewItem> IItemable.Items()
        {
            var items = new List<ListViewItem>();

            var item = new ListViewItem("Version");
            item.SubItems.Add(Header.Offset.ToSize());
            item.SubItems.Add(4L.ToSize());
            item.SubItems.Add(Version.ToString());
            items.Add(item);

            return items;
        }
    }
}
