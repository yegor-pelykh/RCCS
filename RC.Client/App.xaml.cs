using System;
using System.Diagnostics;
using Avalonia;
using Avalonia.Markup.Xaml;
using RC.Client.Connection;
using RC.Client.Storage;
using ClientMessage = RC.Common.Message.ClientMessages;
using ServerMessage = RC.Common.Message.ServerMessages;

namespace RC.Client
{
    public class App : Application
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
            if (Connection == null || Storage == null)
                return;

            Connection.SendMessage(new ClientMessage.Greeting
            {
                Id = Storage.Machine.InstanceId
            });
            var message = Connection.WaitMessage();
            if (message is ServerMessage.Greeting serverGreeting)
            {

            }
        }

        private void ForceExit()
        {
            ReleaseResources();
            Environment.Exit(1);
        }

        private void ReleaseResources()
        {
            Storage?.Dispose();
            Connection?.Dispose();
        }

        #endregion

        #region Properties

        public DataStorage Storage { get; private set; }

        public TLSConnection Connection { get; private set; }

        #endregion

    }

}