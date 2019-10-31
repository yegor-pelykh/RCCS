using System;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;

namespace RC.Client
{
    internal class CommandLineArguments
    {
        internal CommandLineArguments()
        {
            var args = Environment.GetCommandLineArgs()
                .Skip(1)
                .ToArray();
            var cmdApp = new CommandLineApplication();

            var paramMachineConfig = cmdApp.Option<string>("-mc|--machine-config <MACHINECONFIG>", "Machine config", CommandOptionType.SingleValue);
            var paramUserConfig = cmdApp.Option<string>("-uc|--user-config <USERCONFIG>", "User config", CommandOptionType.SingleValue);

            cmdApp.OnExecute(() =>
            {
                MachineConfig = paramMachineConfig.Value();
                UserConfig = paramUserConfig.Value();
            });
            cmdApp.Execute(args);
        }

        #region Properties

        internal string MachineConfig { get; private set; }

        internal string UserConfig { get; private set; }

        #endregion

    }
}