using System;
using RC.Server.Storage.Section;

namespace RC.Server.Storage
{
    public class DataStorage : IDisposable
    {
        public DataStorage()
        {
            Load();
        }

        #region Public Methods

        public void Dispose()
        {
            Machine?.Dispose();
            User?.Dispose();
        }

        #endregion

        #region Private Methods

        private void Load()
        {
            Machine = new MachineSection();
            User = new UserSection();
        }

        #endregion

        #region Properties

        public MachineSection Machine { get; private set; }

        public UserSection User { get; private set; }
        
        #endregion

    }

}
