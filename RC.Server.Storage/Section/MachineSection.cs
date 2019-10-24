using System;
using System.Net;
using Newtonsoft.Json;

namespace RC.Server.Storage.Section
{
    public class MachineSection : ICloneable
    {
        #region Default Value Getters

        internal static MachineSection GetDefault()
        {
            return new MachineSection();
        }

        #endregion

        #region ICloneable Implementation

        public object Clone()
        {
            return new MachineSection();
        }

        #endregion

    }

}
