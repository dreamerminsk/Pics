using System.IO;

namespace Pics.Readers
{
    public class EBMLConstants
    {
        public static long EBMLID = 0x1A45DFA3;
    }

    public class MkvFile
    {
        private FileInfo sourceFile;

        public MkvFile(FileInfo sourceFile)
        {
            this.sourceFile = sourceFile;
        }


    }
}
