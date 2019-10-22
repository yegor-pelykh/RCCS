namespace RC.Client.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            MachineInfo = new MachineInfoViewModel();
        }

        public MachineInfoViewModel MachineInfo { get; }

    }

}
