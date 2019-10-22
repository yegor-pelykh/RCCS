using System;
using System.IO;

namespace RC.Client.Storage.Section
{
    public class UserSection : IDisposable
    {
        public UserSection()
        {
            Directory.CreateDirectory(UserDirectoryPath);
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
        private static readonly string UserDirectoryPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DirectoryName);
        #endregion

        #region Constants
        private const string DirectoryName = "RC";
        #endregion

    }

}
