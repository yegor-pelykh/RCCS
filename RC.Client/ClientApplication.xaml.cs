using System;
using System.Diagnostics;
using Avalonia.Markup.Xaml;
using RC.Client.Connection;
using RC.Client.Storage;
using ClientMessage = RC.Common.Message.ClientMessages;
using ServerMessage = RC.Common.Message.ServerMessages;

namespace RC.Client
{
    public class ClientApplication : Avalonia.Application
    {
        #region Public Methods

        public override void Initialize()
        {
            base.Initialize();
            try
            {
                Storage = new DataStorage();
                Connection = new TLSConnection();
                Greet();
            }
            catch (Exception e)
            {
                Debug.Print(e.Message, e.InnerException?.Message);
                ForceExit();
            }
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnExiting(object sender, EventArgs e)
        {
            ReleaseResources();
            base.OnExiting(sender, e);
        }

        #endregion

        #region Private Methods

        private void Greet()
        {
            if (Storage == null || Connection == null)
                return;

            Connection.SendMessage(new ClientMessage.Greeting
            {
                Id = Storage.Machine.InstanceId
            });
            var message = Connection.WaitMessage();
            if (message is ServerMessage.Greeting serverGreeting)
            {
                Application.Storage.UpdateMachineSection(section =>
                {
                    section.LastKnownIpAddress = serverGreeting.Ip;
                    return true;
                });
            }
        }

        private void ForceExit()
        {
            ReleaseResources();
            Environment.Exit(1);
        }

        private void ReleaseResources()
        {
            Connection?.Dispose();
        }

        #endregion

        #region Properties

        public DataStorage Storage { get; private set; }

        public TLSConnection Connection { get; private set; }

        #endregion

    }

}