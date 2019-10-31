using System;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using RC.Common.Helpers.StaticHelpers;
using RC.Common.Message;
using ClientMessages = RC.Common.Message.ClientMessages;
using ServerMessages = RC.Common.Message.ServerMessages;

namespace RC.Server
{
    internal static class TLSServer
    {
        #region Internal Methods

        internal static void Run(X509Certificate2 certificate)
        {
            _certificate = certificate;

            var localEndPoint = new IPEndPoint(IPAddress.Any, Port);

            var listener = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");

                    var client = listener.Accept();
                    Task.Run(() => ProcessClient(client));
                }

            }
            catch (Exception e)
            {
                ConsoleHelper.PrintException(e);
            }
        }

        internal static void SendMessage(this SslStream stream, ServerMessage message)
        {
            ServerMessage.Send(stream, message);
        }

        internal static ClientMessage ReadMessage(this SslStream stream)
        {
            return ClientMessage.Parse(stream);
        }

        internal static ClientMessage WaitMessage(this SslStream stream)
        {
            do { } while (!stream.CanRead);
            return ReadMessage(stream);
        }

        #endregion

        #region Private Methods

        private static void ProcessClient(Socket client)
        {
            var clientId = Guid.Empty;

            using var ns = new NetworkStream(client, true);
            using var stream = new SslStream(ns, false);

            try
            {
                stream.AuthenticateAsServer(_certificate, clientCertificateRequired: false, checkCertificateRevocation: true);

                stream.ReadTimeout = 5000;
                stream.WriteTimeout = 5000;
                
                clientId = Greet(client, stream);

                CommunicationHandler.OnCommunication(client, stream);
            }
            catch (Exception e)
            {
                ConsoleHelper.PrintException(e);
            }
            finally
            {
                if (clientId != Guid.Empty)
                    ClientManager.RemoveClientInfo(clientId);
                stream.Close();
            }
        }
        
        private static Guid Greet(Socket client, SslStream stream)
        {
            var message = stream.WaitMessage();
            if (message.MessageType != ClientMessageType.Greeting ||
                !(message is ClientMessages.Greeting clientGreeting))
                throw new Exception("The first message from the client should be a greeting.");

            var clientInfo = new ClientInfo(client, stream);
            ClientManager.AddClientInfo(clientGreeting.Id, clientInfo);

            var endPoint = (IPEndPoint)client.RemoteEndPoint;
            stream.SendMessage(new ServerMessages.Greeting
            {
                Ip = endPoint.Address
            });

            return clientGreeting.Id;
        }

        #endregion

        #region Fields

        private static X509Certificate2 _certificate;

        #endregion

        #region Constants

        private const int Port = 12321;

        #endregion

    }

}
