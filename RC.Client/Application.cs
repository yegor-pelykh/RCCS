using RC.Client.Storage;

namespace RC.Client
{
    internal static class Application
    {
        internal static DataStorage Storage => ((ClientApplication) Avalonia.Application.Current).Storage;

    }

}
