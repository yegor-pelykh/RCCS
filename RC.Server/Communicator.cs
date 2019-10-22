using System.Net.Security;
using RC.Common.Message.ServerMessages;

namespace RC.Server
{
    internal static class Communicator
    {
        internal static void OnCommunication(SslStream stream)
        {
            var message = stream.WaitMessage();
            stream.SendMessage(new Greeting());
        }

    }

}
