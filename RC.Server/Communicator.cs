using System.Net.Security;
using System.Net.Sockets;
using RC.Common.Message;
using ClientMessage = RC.Common.Message.ClientMessages;
using ServerMessage = RC.Common.Message.ServerMessages;

namespace RC.Server
{
    internal static class Communicator
    {
        internal static void OnCommunication(TcpClient client, SslStream stream)
        {
            

            var message = stream.WaitMessage();
            switch (message.MessageType)
            {
                case ClientMessageType.Greeting:
                    OnGreeting(client, stream);
                    break;
            }

        }

        private static void OnGreeting(TcpClient client, SslStream stream)
        {
            var endPoint = client.GetClientEndPoint();
            stream.SendMessage(new ServerMessage.Greeting
            {
                Ip = endPoint.Address
            });
        }



    }

}
