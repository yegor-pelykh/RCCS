using System;
using System.Linq;
using McMaster.Extensions.CommandLineUtils;

namespace RC.Server
{
    internal class CommandLineArguments
    {
        internal CommandLineArguments()
        {
            var args = Environment.GetCommandLineArgs()
                .Skip(1)
                .ToArray();
            var cmdApp = new CommandLineApplication();

            var certificatePath = cmdApp.Option<string>("-c|--certificate <CERTIFICATE>", "Server certificate", CommandOptionType.SingleValue);
            var privateKeyPath = cmdApp.Option<string>("-pk|--private-key <PRIVATEKEY>", "Server private key", CommandOptionType.SingleValue);

            cmdApp.OnExecute(() =>
            {
                CertificatePath = certificatePath.Value();
                PrivateKeyPath = privateKeyPath.Value();
            });
            cmdApp.Execute(args);
        }

        #region Properties

        internal string CertificatePath { get; private set; }

        internal string PrivateKeyPath { get; private set; }

        #endregion

    }
}