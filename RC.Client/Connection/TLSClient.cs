using System;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using RC.Common.Helpers.StaticHelpers;
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

            _client = new Socket(SocketType.Stream, ProtocolType.Tcp);

            var remoteEndPoint = new IPEndPoint(ServerIp, ServerPort);
            _client.Connect(remoteEndPoint);

            var ns = new NetworkStream(_client, true);

            _stream = new SslStream(ns, false, OnUserCertificateValidation);
            _stream.AuthenticateAsClient(ServerCertificateName);

            Greet();
        }

        public void Disconnect()
        {
            if (_client != null && _client.Connected)
                _client.Shutdown(SocketShutdown.Both);
            _stream?.Close();
            _client?.Close();
        }

        public void SendMessage(ClientMessage message)
        {
            ClientMessage.Send(_stream, message);
        }
        
        public ServerMessage ReadMessage()
        {
            return ServerMessage.Parse(_stream);
        }
        
        public ServerMessage WaitMessage()
        {
            do { } while (!_stream.CanRead);
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

        private SslStream _stream;
        private Socket _client;

        #endregion

        #region Constants

        private const string ServerCertificateName = "RCServer";
        private const long ServerIp = 16777343;
        private const int ServerPort = 12321;

        #endregion

    }

}
