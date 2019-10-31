using System;
using Newtonsoft.Json;

namespace RC.Client.Storage.Section
{
    public class UserSection : ICloneable
    {
        #region Ignorable Properties

        [JsonIgnore]
        public string ConfigPath { get; set; }

        #endregion

        #region Default Value Getters

        internal static UserSection GetDefault()
        {
            return new UserSection();
        }

        #endregion

        #region ICloneable Implementation

        public object Clone()
        {
            return new UserSection();
        }

        #endregion

    }

}
