using System.IO;

namespace RC.Common.Helpers
{
    public static class StreamHelper
    {
        public static string ReadStringEx(this BinaryReader reader)
        {
            var length = reader.ReadInt32();
            return new string(reader.ReadChars(length));
        }

        public static void WriteStringEx(this BinaryWriter writer, string data)
        {
            var chars = data.ToCharArray();
            writer.Write(chars.Length);
            writer.Write(chars);
        }

    }

}
