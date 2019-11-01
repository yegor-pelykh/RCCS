using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using RC.Common.Certification;
using RC.Common.Helpers.StaticHelpers;

namespace RC.Server
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                CommandLineArguments = new CommandLineArguments();

                if (CommandLineArguments.CertificatePath == null)
                    throw new Exception("No certificate file specified.");

                if (CommandLineArguments.PrivateKeyPath == null)
                    throw new Exception("No private key file specified.");

                var certificate = Certification.CreateCertificateWithPrivateKey(CommandLineArguments.CertificatePath,
                    CommandLineArguments.PrivateKeyPath);

                TLSServer.Run(certificate);
            }
            catch (Exception e)
            {
                ConsoleHelper.PrintException(e);

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }

        #region Properties

        internal static CommandLineArguments CommandLineArguments { get; private set; }

        #endregion


    }

}
