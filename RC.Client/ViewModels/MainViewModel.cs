namespace RC.Client.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            DeviceInfo = new DeviceInfoViewModel();
        }

        public DeviceInfoViewModel DeviceInfo { get; }

    }

}
