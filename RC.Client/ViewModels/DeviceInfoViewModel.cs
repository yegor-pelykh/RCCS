using System;
using System.Net;

namespace RC.Client.ViewModels
{
    public class DeviceInfoViewModel : ViewModelBase
    {
        public DeviceInfoViewModel()
        {
            InstanceId = Application.Storage.Machine.InstanceId;
            IpAddress = Application.Storage.Machine.LastKnownIpAddress;
        }

        #region Properties

        public Guid InstanceId { get; private set; }

        public IPAddress IpAddress { get; private set; }

        #endregion

    }

}
