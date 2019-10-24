using System;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using RC.Common.Message;

namespace RC.Server
{
    internal static class TLSServer
    {
        #region Public Methods

        public static void Run(X509Certificate2 certificate, CommunicationHandler communicationHandler)
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

        #endregion

        #region Private Methods

        private static void ProcessClient(TcpClient client, CommunicationHandler communicationHandler)
        {
            var stream = new SslStream(client.GetStream(), false);
            try
            {
                stream.AuthenticateAsServer(_certificate, clientCertificateRequired: false, checkCertificateRevocation: true);

                stream.ReadTimeout = 5000;
                stream.WriteTimeout = 5000;

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
                stream.Close();
                client.Close();
            }
        }

        public static void SendMessage(this SslStream stream, ServerMessage message)
        {
            ServerMessage.Send(stream, message);
        }

        public static ClientMessage ReadMessage(this SslStream stream)
        {
            return ClientMessage.Parse(stream);
        }
        
        public static ClientMessage WaitMessage(this SslStream stream)
        {
            do { } while (!stream.CanRead);
            return ReadMessage(stream);
        }

        public static IPEndPoint GetClientEndPoint(this TcpClient client)
        {
            return client.Client.RemoteEndPoint as IPEndPoint;
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
