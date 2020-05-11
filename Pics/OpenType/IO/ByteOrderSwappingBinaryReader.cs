using System;
using System.IO;

namespace Pics.OpenType.IO
{
    public class ByteOrderSwappingBinaryReader : BinaryReader
    {
        public ByteOrderSwappingBinaryReader(Stream input) : base(input)
        {
        }
        protected override void Dispose(bool disposing)
        {
            GC.SuppressFinalize(this);
            base.Dispose(disposing);
        }

        public override short ReadInt16()
        {
            return BitConverter.ToInt16(RR(2), 8 - 2);
        }

        public override ushort ReadUInt16()
        {
            return BitConverter.ToUInt16(RR(2), 8 - 2);
        }

        public override uint ReadUInt32()
        {
            return BitConverter.ToUInt32(RR(4), 8 - 4);
        }

        public override ulong ReadUInt64()
        {
            return BitConverter.ToUInt64(RR(8), 8 - 8);
        }

        public override double ReadDouble()
        {
            return BitConverter.ToDouble(RR(8), 8 - 8);
        }

        public override int ReadInt32()
        {
            return BitConverter.ToInt32(RR(4), 8 - 4);
        }

        byte[] _reusable_buffer = new byte[8];

        private byte[] RR(int count)
        {
            base.Read(_reusable_buffer, 0, count);
            Array.Reverse(_reusable_buffer);
            return _reusable_buffer;
        }
    }
}
