using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace RC.Server
{
    internal static class ClientManager
    {
        #region Internal methods
        
        internal static void AddClient(Guid id, TcpClient client)
        {
            if (Clients.TryGetValue(id, out var currentClient) && !ReferenceEquals(currentClient, client))
                CloseClient(currentClient);
            Clients[id] = client;
        }
        
        internal static void RemoveClient(Guid id)
        {
            if (Clients.TryGetValue(id, out var currentClient))
                CloseClient(currentClient);
            Clients.Remove(id);
        }

        internal static void CloseClient(TcpClient client)
        {
            try
            {
                var stream = client.GetStream();
                stream.Close();
            }
            catch
            {
                // ignored
            }

            client.Close();
        }

        #endregion

        #region Static Readonly Fields

        private static readonly Dictionary<Guid, TcpClient> Clients = new Dictionary<Guid, TcpClient>();

        #endregion


    }

}
