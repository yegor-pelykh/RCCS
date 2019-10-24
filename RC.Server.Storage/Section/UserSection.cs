using System;

namespace RC.Server.Storage.Section
{
    public class UserSection : ICloneable
    {
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
