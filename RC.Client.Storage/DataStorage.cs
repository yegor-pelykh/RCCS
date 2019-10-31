using System;
using System.IO;
using Newtonsoft.Json;
using RC.Client.Storage.Section;

namespace RC.Client.Storage
{
    public class DataStorage
    {
        public DataStorage(string machineConfigPath = null, string userConfigPath = null)
        {
            Directory.CreateDirectory(MachineDirectoryPath);
            Directory.CreateDirectory(UserDirectoryPath);

            LoadMachineSection(machineConfigPath);
            LoadUserSection(userConfigPath);
        }

        #region Public Methods

        public void UpdateMachineSection(ChangingMachineSectionCallback callback)
        {
            var clone = Machine.Clone() as MachineSection;
            if (!callback.Invoke(clone))
                return;

            Machine = clone;
            SaveMachineSection();
        }

        public void UpdateUserSection(ChangingUserSectionCallback callback)
        {
            var clone = User.Clone() as UserSection;
            if (!callback.Invoke(clone))
                return;

            User = clone;
            SaveUserSection();
        }

        #endregion

        #region Internal Methods

        internal void LoadMachineSection(string path = null)
        {
            if (Machine != null)
                SaveMachineSection();

            var configPath = File.Exists(path)
                ? path
                : MachineStoragePath;

            try
            {
                var contentsMachine = File.ReadAllText(configPath);
                Machine = JsonConvert.DeserializeObject<MachineSection>(contentsMachine);
                Machine.ConfigPath = configPath;
            }
            catch (FileNotFoundException)
            {
                Machine = MachineSection.GetDefault();
                Machine.ConfigPath = configPath;
                SaveMachineSection();
            }
        }

        internal void LoadUserSection(string path = null)
        {
            if (User != null)
                SaveUserSection();

            var configPath = File.Exists(path)
                ? path
                : UserStoragePath;

            try
            {
                var contentsUser = File.ReadAllText(UserStoragePath);
                User = JsonConvert.DeserializeObject<UserSection>(contentsUser);
                User.ConfigPath = configPath;
            }
            catch (FileNotFoundException)
            {
                User = UserSection.GetDefault();
                User.ConfigPath = configPath;
                SaveUserSection();
            }
        }

        internal void SaveMachineSection()
        {
            if (Machine.ConfigPath == null)
                return;

            var contentsMachine = JsonConvert.SerializeObject(Machine);
            File.WriteAllText(Machine.ConfigPath, contentsMachine);
        }

        internal void SaveUserSection()
        {
            if (User.ConfigPath == null)
                return;

            var contentsUser = JsonConvert.SerializeObject(User);
            File.WriteAllText(User.ConfigPath, contentsUser);
        }

        #endregion

        #region Properties

        public MachineSection Machine { get; private set; }

        public UserSection User { get; private set; }

        #endregion

        #region Static Readonly Fields

        private static readonly string MachineDirectoryPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), DirectoryName);
        private static readonly string UserDirectoryPath =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), DirectoryName);
        
        private static readonly string MachineStoragePath =
            Path.Combine(MachineDirectoryPath, StorageFileName);
        private static readonly string UserStoragePath =
            Path.Combine(UserDirectoryPath, StorageFileName);

        #endregion

        #region Constants

        private const string DirectoryName = "RC";
        private const string StorageFileName = "client.storage";

        #endregion

        #region Delegates Definitions

        public delegate bool ChangingMachineSectionCallback(MachineSection section);

        public delegate bool ChangingUserSectionCallback(UserSection section);

        #endregion

    }

}
