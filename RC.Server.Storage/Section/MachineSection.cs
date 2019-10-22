using System;
using System.IO;

namespace RC.Server.Storage.Section
{
    public class MachineSection : IDisposable
    {
        public MachineSection()
        {
            Directory.CreateDirectory(MachineDirectoryPath);
        }

        #region Properties

        #endregion

        #region Public Methods

        public void Dispose()
        {
        }

        #endregion

        #region Private Methods

        #endregion

        #region Fields

        #endregion

        #region Static Readonly Fields

        private static readonly string MachineDirectoryPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), DirectoryName);
        
        #endregion

        #region Constants

        private const string DirectoryName = "RC";
        
        #endregion

    }

}
