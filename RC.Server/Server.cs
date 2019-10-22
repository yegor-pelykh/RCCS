using System;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using RC.Common.Message;
using RC.Common.Message.ServerMessages;

namespace RC.Server
{
    internal static class Server
    {
        public static void Run(X509Certificate2 certificate)
        {
            _certificate = certificate;

            var listener = new TcpListener(IPAddress.Any, Port);
            listener.Start();

            while (true)
            {
                Console.WriteLine("Waiting for a client to connect...");
                var client = listener.AcceptTcpClient();
                Task.Run(() => ProcessClient(client));
            }
        }

        private static void ProcessClient(TcpClient client)
        {
            var sslStream = new SslStream(client.GetStream(), false);
            try
            {
                sslStream.AuthenticateAsServer(_certificate, clientCertificateRequired: false, checkCertificateRevocation: true);

                sslStream.ReadTimeout = 5000;
                sslStream.WriteTimeout = 5000;

                var message = ClientMessage.Parse(sslStream);
                ServerMessage.Send(sslStream, new Greeting());
            }
            catch (AuthenticationException e)
            {
                Console.WriteLine("Exception: {0}", e.Message);
                if (e.InnerException != null)
                    Console.WriteLine("Inner exception: {0}", e.InnerException.Message);
                Console.WriteLine("Authentication failed - closing the connection.");
            }
            finally
            {
                sslStream.Close();
                client.Close();
            }
        }

        #region Fields

        private static X509Certificate2 _certificate;

        #endregion

        #region Constants

        private const int Port = 12321;

        #endregion

    }

}
