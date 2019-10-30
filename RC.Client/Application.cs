using RC.Client.Storage;

namespace RC.Client
{
    internal static class Application
    {
        internal static ClientApplication Instance => (ClientApplication) Avalonia.Application.Current;

    }

}
