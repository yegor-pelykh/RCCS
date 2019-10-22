using System;
using System.Diagnostics;
using System.IO;

namespace RC.Client.Storage.Section
{
    public class MachineSection : IDisposable
    {
        public MachineSection()
        {
            Directory.CreateDirectory(MachineDirectoryPath);

            LoadIdentification();
        }

        #region Properties

        public Guid InstanceId
        {
            get => _instanceId;
            set
            {
                _instanceId = value;
                SaveIdentification();
            }
        }

        #endregion

        #region Public Methods

        public void Dispose()
        {
        }

        #endregion

        #region Private Methods

        private void LoadIdentification()
        {
            try
            {
                var strGuid = File.ReadAllText(IdentificationFilePath);
                InstanceId = Guid.Parse(strGuid);
            }
            catch (Exception e)
            {
                InstanceId = Guid.NewGuid();

                // TODO: It should be desktop notification
                Debug.Print(e.Message, e.InnerException?.Message);
                Debug.Print("Instance Id was regenerated.");
            }
        }

        private void SaveIdentification()
        {
            File.WriteAllText(IdentificationFilePath, InstanceId.ToString());
        }

        #endregion

        #region Fields

        private Guid _instanceId;
        
        #endregion

        #region Static Readonly Fields

        private static readonly string MachineDirectoryPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), DirectoryName);
        private static readonly string IdentificationFilePath =
            Path.Combine(MachineDirectoryPath, IdentificationFileName);
        
        #endregion

        #region Constants

        private const string DirectoryName = "RC";
        private const string IdentificationFileName = "id.storage";
        
        #endregion

    }

}
