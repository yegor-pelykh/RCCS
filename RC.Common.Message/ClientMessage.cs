using System;
using System.IO;
using System.Text;
using RC.Common.Message.ClientMessages;

namespace RC.Common.Message
{
    public abstract class ClientMessage : Message
    {
        public abstract ClientMessageType MessageType { get; }

        public static void Send(Stream stream, ClientMessage message)
        {
            if (stream == null || message == null)
                return;

            using var writer = new BinaryWriter(stream, Encoding.UTF8, true);
            writer.Write((byte)message.MessageType);
            message.Serialize(writer);
        }

        public static ClientMessage Parse(Stream stream)
        {
            if (stream == null)
                return null;

            ClientMessage message;
            using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
            {
                var messageTypeValue = reader.ReadByte();
                if (!Enum.IsDefined(typeof(ClientMessageType), messageTypeValue))
                    throw new Exception($"Unknown client message type: {messageTypeValue}.");

                var messageType = (ClientMessageType) messageTypeValue;
                switch (messageType)
                {
                    case ClientMessageType.Greeting:
                        message = new Greeting();
                        break;
                    default:
                        throw new Exception($"No responsible class defined for client message type {messageType}.");
                }
                message.Deserialize(reader);
            }
            return message;
        }

    }

}
