using System.Text;

namespace Pics.Readers.OpenType {
	public class TableRecord {
		public string tableTag;

		public uint checkSum;

		public uint offset;

		public uint length;

		public static TableRecord[] ReadArray(BinaryReaderFont reader, int count) {
			TableRecord[] array = new TableRecord[count];
			for (int i = 0; i < count; i++) {
				array[i] = Read(reader);
			}
			return array;
		}

		public static TableRecord Read(BinaryReaderFont reader) {
			return new TableRecord {
				tableTag = reader.ReadTag(),
				checkSum = reader.ReadUInt32(),
				offset = reader.ReadUInt32(),
				length = reader.ReadUInt32()
			};
		}

		public override string ToString() {
			StringBuilder builder = new StringBuilder();
			builder.AppendLine("{");
			builder.AppendFormat("\t\"tableTag\": \"{0}\",\n", tableTag);
			builder.AppendFormat("\t\"checkSum\": 0x{0:X8},\n", checkSum);
			builder.AppendFormat("\t\"offset\": 0x{0:X8},\n", offset);
			builder.AppendFormat("\t\"length\": 0x{0:X8}\n", length);
			builder.Append("}");
			return builder.ToString();
		}
	}
}
