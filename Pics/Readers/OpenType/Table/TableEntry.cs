using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pics.Readers.OpenType.Table
{
    public abstract class TableEntry
    {
        public TableEntry()
        {
        }
        public TableHeader Header { get; set; }
        public abstract void ReadFrom(BinaryReader reader);
        public abstract string Name { get; }
        public uint TableLength => this.Header.Length;


    }
}
