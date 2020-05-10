using System.IO;

namespace Pics.OpenType
{
    public static class Extensions
    {

        public static readonly string SIZE_FORMAT = "###,###";

        public static string ReadTag(this BinaryReader reader)
        {
            string tag = "";
            for (var i = 0; i < 4; i++)
            {
                tag += (char)reader.ReadByte();
            }
            return tag;
        }

        public static string ToSize(this long number)
        {
            return number.ToString(SIZE_FORMAT);
        }

        public static string ToSize(this uint number)
        {
            return number.ToString(SIZE_FORMAT);
        }
    }
}
