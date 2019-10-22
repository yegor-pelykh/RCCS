using System;
using System.IO;
using System.Text;
using RC.Common.Message.ServerMessages;

namespace RC.Common.Message
{
    public abstract class ServerMessage : Message
    {
        public abstract ServerMessageType MessageType { get; }

        public static void Send(Stream stream, ServerMessage message)
        {
            if (stream == null || message == null)
                return;

            using var writer = new BinaryWriter(stream, Encoding.UTF8, true);
            writer.Write((byte) message.MessageType);
            message.Serialize(writer);
        }

        public static ServerMessage Parse(Stream stream)
        {
            if (stream == null)
                return null;

            ServerMessage message;
            using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
            {
                var messageTypeValue = reader.ReadByte();
                if (!Enum.IsDefined(typeof(ServerMessageType), messageTypeValue))
                    throw new Exception($"Unknown server message type: {messageTypeValue}.");

                var messageType = (ServerMessageType)messageTypeValue;
                switch (messageType)
                {
                    case ServerMessageType.Greeting:
                        message = new Greeting();
                        break;
                    default:
                        throw new Exception($"No responsible class defined for server message type {messageType}.");
                }
                message.Deserialize(reader);
            }
            return message;
        }

    }

}
