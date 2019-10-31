using System.Net.Security;
using System.Net.Sockets;

namespace RC.Server
{
    internal class ClientInfo
    {
        internal ClientInfo(Socket client, SslStream stream)
        {
            Client = client;
            Stream = stream;
        }

        internal Socket Client { get; set; }

        internal SslStream Stream { get; set; }

    }

}
