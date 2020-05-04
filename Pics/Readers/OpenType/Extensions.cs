using System.IO;

namespace Pics.Readers.OpenType
{
    public static class Extensions
    {
        public static string ReadTag(this BinaryReader reader)
        {
            string tag = "";
            for (var i = 0; i < 4; i++)
            {
                tag += (char)reader.ReadByte();
            }
            return tag;
        }
    }
}
