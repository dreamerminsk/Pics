using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pics.Readers.OpenType.Table
{
    public class HeadTable : TableEntry
    {
        public override string Name => "head";

        public uint Version { get; private set; }
        public uint FontRevision { get; private set; }
        public uint CheckSumAdjustment { get; private set; }
        public uint MagicNumber { get; private set; }
        public ushort Flags { get; private set; }
        public ushort UnitsPerEm { get; private set; }
        public ulong Created { get; private set; }
        public ulong Modified { get; private set; }
        public Bounds Bounds => _bounds;
        public ushort MacStyle { get; private set; }
        public ushort LowestRecPPEM { get; private set; }
        public short FontDirectionHint { get; private set; }
        public bool WideGlyphLocations => _indexToLocFormat > 0;
        public short GlyphDataFormat { get; private set; }

        public override void ReadFrom(BinaryReader reader)
        {
            Version = reader.ReadUInt32();
            FontRevision = reader.ReadUInt32();
            CheckSumAdjustment = reader.ReadUInt32();
            MagicNumber = reader.ReadUInt32();
            //if (MagicNumber != 0x5F0F3CF5) throw new Exception("Invalid magic number!" + MagicNumber.ToString("x"));
            Flags = reader.ReadUInt16();
            UnitsPerEm = reader.ReadUInt16();
            Created = reader.ReadUInt64();
            Modified = reader.ReadUInt64();
            _bounds = Utils.ReadBounds(reader);
            MacStyle = reader.ReadUInt16();
            LowestRecPPEM = reader.ReadUInt16();
            FontDirectionHint = reader.ReadInt16();
            _indexToLocFormat = reader.ReadInt16();
            GlyphDataFormat = reader.ReadInt16();
        }
    }
}
