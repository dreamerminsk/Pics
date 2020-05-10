using Pics.View;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Pics.OpenType.Table
{
    public class FpgmTable : TableEntry, IItemable
    {
        private TableHeader header;

        public FpgmTable(TableHeader header) : base(header)
        {
            this.header = header;
        }

        public override string Name => "fpgm";

        public byte[] instructions { get; private set; }

        public List<ListViewItem> Items()
        {
            var items = new List<ListViewItem>();

            for (var i = 0; i < instructions.Length; i++)
            {
                var opcode = (OpCode)instructions[i];
                switch (opcode)
                {
                    case OpCode.NPUSHB:
                    case OpCode.PUSHB1:
                    case OpCode.PUSHB2:
                    case OpCode.PUSHB3:
                    case OpCode.PUSHB4:
                    case OpCode.PUSHB5:
                    case OpCode.PUSHB6:
                    case OpCode.PUSHB7:
                    case OpCode.PUSHB8:
                        {
                            var pushbCode = new ListViewItem("OpCode: " + opcode.ToString());
                            pushbCode.SubItems.Add((Header.Offset + i).ToSize());
                            pushbCode.SubItems.Add(1L.ToSize());
                            items.Add(pushbCode);
                            var count = opcode == OpCode.NPUSHB ? instructions[++i] : opcode - OpCode.PUSHB1 + 1;
                            for (int j = 0; j < count; j++)
                            {
                                var byteItem = new ListViewItem("Data: " + instructions[++i]);
                                byteItem.SubItems.Add((Header.Offset + i).ToSize());
                                byteItem.SubItems.Add(1L.ToSize());
                                items.Add(byteItem);
                            }
                        }
                        break;
                    case OpCode.NPUSHW:
                    case OpCode.PUSHW1:
                    case OpCode.PUSHW2:
                    case OpCode.PUSHW3:
                    case OpCode.PUSHW4:
                    case OpCode.PUSHW5:
                    case OpCode.PUSHW6:
                    case OpCode.PUSHW7:
                    case OpCode.PUSHW8:
                        {
                            var pushCode = new ListViewItem("OpCode: " + opcode.ToString());
                            pushCode.SubItems.Add((Header.Offset + i).ToSize());
                            pushCode.SubItems.Add(1L.ToSize());
                            items.Add(pushCode);
                            var count = opcode == OpCode.NPUSHW ? instructions[++i] : opcode - OpCode.PUSHW1 + 1;
                            for (int j = 0; j < count; j++)
                            {
                                var dataItem = new ListViewItem("Data: " + instructions[++i]);
                                dataItem.SubItems.Add((Header.Offset + i).ToSize());
                                dataItem.SubItems.Add(1L.ToSize());
                                items.Add(dataItem);

                                dataItem = new ListViewItem("Data: " + instructions[++i]);
                                dataItem.SubItems.Add((Header.Offset + i).ToSize());
                                dataItem.SubItems.Add(1L.ToSize());
                                items.Add(dataItem);
                            }
                        }
                        break;
                    case OpCode.FDEF:
                    case OpCode.ENDF:
                    case OpCode.MPPEM:
                    case OpCode.LT:
                    case OpCode.IF:
                    case OpCode.INSTCTRL:
                        var itm = new ListViewItem("OpCode: " + opcode.ToString());
                        itm.SubItems.Add((Header.Offset + i).ToSize());
                        itm.SubItems.Add(1L.ToSize());
                        items.Add(itm);
                        break;
                    default:
                        var item = new ListViewItem("OpCode: " + instructions[i].ToString("X") + " - " + opcode.ToString());
                        item.SubItems.Add((Header.Offset + i).ToSize());
                        item.SubItems.Add(1L.ToSize());
                        items.Add(item);
                        break;
                }

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