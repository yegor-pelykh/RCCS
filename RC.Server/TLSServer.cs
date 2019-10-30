using System;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using RC.Common.Message;
using ClientMessages = RC.Common.Message.ClientMessages;
using ServerMessages = RC.Common.Message.ServerMessages;

namespace RC.Server
{
    internal static class TLSServer
    {
        #region Internal Methods

        internal static void Run(X509Certificate2 certificate, CommunicationHandler communicationHandler)
        {
            _certificate = certificate;

            var listener = new TcpListener(IPAddress.Any, Port);
            listener.Start();

            while (true)
            {
                Console.WriteLine("Waiting for a client to connect...");
                var client = listener.AcceptTcpClient();
                Task.Run(() => ProcessClient(client, communicationHandler));
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

        internal static IPEndPoint GetClientEndPoint(this TcpClient client)
        {
            return client.Client.RemoteEndPoint as IPEndPoint;
        }

        #endregion

        #region Private Methods

        private static void ProcessClient(TcpClient client, CommunicationHandler communicationHandler)
        {
            var clientId = Guid.Empty;
            var stream = new SslStream(client.GetStream(), false);
            try
            {
                stream.AuthenticateAsServer(_certificate, clientCertificateRequired: false, checkCertificateRevocation: true);

                stream.ReadTimeout = 5000;
                stream.WriteTimeout = 5000;

                clientId = Greet(client, stream);

                communicationHandler?.Invoke(client, stream);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
                if (e.InnerException != null)
                    Console.WriteLine("Inner exception: {0}", e.InnerException.Message);
            }
            finally
            {
                if (clientId != Guid.Empty)
                    ClientManager.RemoveClient(clientId);
                ClientManager.CloseClient(client);
            }
        }

        private static Guid Greet(TcpClient client, SslStream stream)
        {
            var message = stream.WaitMessage();
            if (message.MessageType != ClientMessageType.Greeting ||
                !(message is ClientMessages.Greeting clientGreeting))
                throw new Exception("The first message from the client should be a greeting.");

            var id = clientGreeting.Id;
            ClientManager.AddClient(clientGreeting.Id, client);

            var endPoint = client.GetClientEndPoint();
            stream.SendMessage(new ServerMessages.Greeting
            {
                Ip = endPoint.Address
            });

            return id;
        }

        #endregion

        #region Fields

        private static X509Certificate2 _certificate;

        #endregion

        #region Constants

        private const int Port = 12321;

        #endregion

        #region Delegate Definitions

        internal delegate void CommunicationHandler(TcpClient client, SslStream stream);

        #endregion

    }

}
