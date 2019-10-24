using Avalonia;
using Avalonia.Logging.Serilog;
using RC.Client.Views;

namespace RC.Client
{
    internal class Program
    {
        public static void Main(string[] args) => BuildAvaloniaApp().Start(AppMain, args);

        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<ClientApplication>()
                .UsePlatformDetect()
                .LogToDebug()
                .UseReactiveUI();

        private static void AppMain(Avalonia.Application app, string[] args)
        {
            var mainWindow = new MainWindow();
            app.Run(mainWindow);
        }

    }

}
