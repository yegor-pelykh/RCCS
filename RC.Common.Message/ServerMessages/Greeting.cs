using System.IO;
using System.Net;

namespace RC.Common.Message.ServerMessages
{
    public class Greeting : ServerMessage
    {
        public override ServerMessageType MessageType { get; } = ServerMessageType.Greeting;

        #region Properties

        public IPAddress Ip { get; set; }

        #endregion

        #region Overriden Methods

        public override void Serialize(BinaryWriter writer)
        {
            var bytes = Ip.GetAddressBytes();
            var length = (byte) bytes.Length;

            writer.Write(length);
            writer.Write(bytes);
        }

        public override void Deserialize(BinaryReader reader)
        {
            var length = reader.ReadByte();
            var bytes = reader.ReadBytes(length);

            Ip = new IPAddress(bytes);
        }

        #endregion

    }

}
