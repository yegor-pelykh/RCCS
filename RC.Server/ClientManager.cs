using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace RC.Server
{
    internal static class ClientManager
    {
        #region Internal methods
        
        internal static void AddClientInfo(Guid id, ClientInfo info)
        {
            Clients[id] = info;
        }
        
        internal static void RemoveClientInfo(Guid id)
        {
            Clients.Remove(id);
        }

        internal static ClientInfo GetClientInfo(Guid id)
        {
            return Clients.TryGetValue(id, out var info) ? info : null;
        }

        #endregion

        #region Static Readonly Fields

        private static readonly Dictionary<Guid, ClientInfo> Clients = new Dictionary<Guid, ClientInfo>();

        #endregion


    }

}
