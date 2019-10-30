using System;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using RC.Common.Message;
using ClientMessages = RC.Common.Message.ClientMessages;
using ServerMessages = RC.Common.Message.ServerMessages;

namespace RC.Client.Connection
{
    public class TLSClient
    {
        #region Public Methods

        public void Connect()
        {
            Disconnect();

            _client = new TcpClient();
            _client.Connect(new IPAddress(ServerIp), ServerPort);

            _sslStream = new SslStream(_client.GetStream(), false, OnUserCertificateValidation);
            _sslStream.AuthenticateAsClient(ServerCertificateName);

            Greet();
        }

        public void Disconnect()
        {
            try
            {
                _sslStream?.Close();
            }
            catch
            {
                // ignored
            }

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
        
        public ServerMessage WaitMessage()
        {
            do { } while (!_sslStream.CanRead);
            return ReadMessage();
        }

        #endregion

        #region Private Methods
        
        private void Greet()
        {
            var storage = Application.Instance.Storage;
            if (storage == null)
                return;

            SendMessage(new ClientMessages.Greeting
            {
                Id = storage.Machine.InstanceId
            });

            var message = WaitMessage();
            if (message.MessageType != ServerMessageType.Greeting ||
                !(message is ServerMessages.Greeting serverGreeting))
                throw new Exception("The first message from the server should be a greeting.");

            storage.UpdateMachineSection(section =>
            {
                section.LastKnownIpAddress = serverGreeting.Ip;
                return true;
            });
        }

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
