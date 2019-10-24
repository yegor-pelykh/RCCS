using System;
using System.IO;
using Newtonsoft.Json;
using RC.Server.Storage.Section;

namespace RC.Server.Storage
{
    public class DataStorage
    {
        public DataStorage()
        {
            Directory.CreateDirectory(MachineDirectoryPath);
            Directory.CreateDirectory(UserDirectoryPath);

            LoadMachineSection();
            LoadUserSection();
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

        internal void LoadMachineSection()
        {
            try
            {
                var contentsMachine = File.ReadAllText(MachineStoragePath);
                Machine = JsonConvert.DeserializeObject<MachineSection>(contentsMachine);
            }
            catch (FileNotFoundException)
            {
                Machine = MachineSection.GetDefault();
                SaveMachineSection();
            }
        }

        internal void LoadUserSection()
        {
            try
            {
                var contentsUser = File.ReadAllText(UserStoragePath);
                User = JsonConvert.DeserializeObject<UserSection>(contentsUser);
            }
            catch (FileNotFoundException)
            {
                User = UserSection.GetDefault();
                SaveUserSection();
            }
        }

        internal void SaveMachineSection()
        {
            var contentsMachine = JsonConvert.SerializeObject(Machine);
            File.WriteAllText(MachineStoragePath, contentsMachine);
        }

        internal void SaveUserSection()
        {
            var contentsUser = JsonConvert.SerializeObject(User);
            File.WriteAllText(UserStoragePath, contentsUser);
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
        private const string StorageFileName = "server.storage";

        #endregion

        #region Delegates Definitions

        public delegate bool ChangingMachineSectionCallback(MachineSection section);

        public delegate bool ChangingUserSectionCallback(UserSection section);

        #endregion

    }

}
