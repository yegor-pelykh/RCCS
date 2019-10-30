using System;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using RC.Common.Certification;

namespace RC.Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args == null || args.Length < 2)
            {
                Console.WriteLine("No certificate file or private key file specified.");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return;
            }
            
            var certificatePath = args[0];
            var keyPath = args[1];

            X509Certificate2 certificate;
            try
            {
                certificate = Certification.CreateCertificateWithPrivateKey(certificatePath, keyPath);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error loading certificate or private key.");
                Console.WriteLine($"Exception: {e.Message}");
                return;
            }

            TLSServer.Run(certificate, CommunicationHandler.OnCommunication);
        }

    }

}
