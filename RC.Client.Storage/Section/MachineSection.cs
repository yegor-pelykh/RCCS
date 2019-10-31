using System;
using System.Net;
using Newtonsoft.Json;
using RC.Client.Storage.Converters;

namespace RC.Client.Storage.Section
{
    public class MachineSection : ICloneable
    {
        #region Properties

        public Guid InstanceId { get; set; }

        [JsonConverter(typeof(IPAddressConverter))]
        public IPAddress LastKnownIpAddress { get; set; }

        #endregion

        #region Ignorable Properties
        
        [JsonIgnore]
        public string ConfigPath { get; set; }

        #endregion

        #region Default Value Getters

        internal static MachineSection GetDefault()
        {
            return new MachineSection
            {
                InstanceId = GetDefaultInstanceId(),
                LastKnownIpAddress = GetDefaultLastKnownIpAddress()
            };
        }

        private static Guid GetDefaultInstanceId()
        {
            return Guid.NewGuid();
        }

        private static IPAddress GetDefaultLastKnownIpAddress()
        {
            return null;
        }

        #endregion

        #region ICloneable Implementation

        public object Clone()
        {
            return new MachineSection
            {
                InstanceId = InstanceId,
                LastKnownIpAddress = LastKnownIpAddress
            };
        }

        #endregion

    }

}
