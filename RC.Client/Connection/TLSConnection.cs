using System;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using RC.Common.Message;

namespace RC.Client.Connection
{
    public class TLSConnection : IDisposable
    {
        public TLSConnection()
        {
            Connect();
        }

        #region Public Methods

        public void Connect()
        {
            Disconnect();

            _client = new TcpClient();
            _client.Connect(new IPAddress(ServerIp), ServerPort);

            _sslStream = new SslStream(_client.GetStream(), false, OnUserCertificateValidation);
            _sslStream.AuthenticateAsClient(ServerCertificateName);
        }

        public void Disconnect()
        {
            _sslStream?.Close();
            _client?.Close();
        }

        public void SendMessage(ClientMessage message)
        {
            ClientMessage.Send(_sslStream, message);
        }
        
        public ServerMessage ReadMessage()
        {
            return ServerMessage.Parse(_sslStream);
        }

        public void Dispose()
        {
            Disconnect();
        }

        #endregion

        #region Private Methods

        private static bool OnUserCertificateValidation(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return errors == SslPolicyErrors.None || errors == SslPolicyErrors.RemoteCertificateChainErrors;
        }

        #endregion

        #region Fields

        private SslStream _sslStream;
        private TcpClient _client;

        #endregion

        #region Constants

        private const string ServerCertificateName = "RCServer";
        private const long ServerIp = 16777343;
        private const int ServerPort = 12321;

        #endregion

    }

}
