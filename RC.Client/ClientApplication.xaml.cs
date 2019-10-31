using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Markup.Xaml;
using RC.Client.Connection;
using RC.Client.Storage;

namespace RC.Client
{
    public class ClientApplication : Avalonia.Application
    {
        #region Public Methods

        public override void Initialize()
        {
            base.Initialize();
            LoadCriticalComponents();
            LoadUserInterface();
            Task.Run(ReconnectToServer);
        }
        
        protected override void OnExiting(object sender, EventArgs e)
        {
            ReleaseResources();
            base.OnExiting(sender, e);
        }

        #endregion

        #region Private Methods

        private void LoadCriticalComponents()
        {
            try
            {
                Storage = new DataStorage();
                Connection = new TLSClient();
            }
            catch (Exception e)
            {
                Debug.Print(e.Message, e.InnerException?.Message);
                ForceExit();
            }
        }
        
        private void LoadUserInterface()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void ForceExit()
        {
            ReleaseResources();
            Environment.Exit(1);
        }

        private void ReleaseResources()
        {
            DisconnectFromServer();
        }

        private void ReconnectToServer()
        {
            var isReconnecting = false;
            while (!IsConnected)
            {
                if (isReconnecting)
                {
                    Task.Run(() =>
                    {
                        for (ReconnectionTime = StartReconnectionTime;
                            ReconnectionTime > 0;
                            ReconnectionTime -= ReconnectionTimeout)
                            Thread.Sleep(ReconnectionTimeout);
                    }).Wait();
                }
                isReconnecting = true;
                ConnectToServer();
            }
        }

        private void ConnectToServer()
        {
            try
            {
                Connection.Connect();
                IsConnected = true;
            }
            catch
            {
                DisconnectFromServer();
            }
        }

        private void DisconnectFromServer()
        {
            Connection.Disconnect();
            IsConnected = false;
        }

        #endregion

        #region Properties

        public DataStorage Storage { get; private set; }

        public TLSClient Connection { get; private set; }

        public bool IsConnected
        {
            get => _isConnected;
            private set
            {
                if (value == _isConnected)
                    return;

                _isConnected = value;
                ConnectionStateChanged?.Invoke(this, new ConnectionStateChangedEventArgs(_isConnected));
            }
        }

        public int ReconnectionTime
        {
            get => _reconnectionTime;
            private set
            {
                if (value == _reconnectionTime)
                    return;

                _reconnectionTime = value;
                ReconnectionTimeChanged?.Invoke(this, new ReconnectionTimeChangedEventArgs(_reconnectionTime));
            }
        }

        #endregion

        #region Private Fields

        private bool _isConnected;
        private int _reconnectionTime;

        #endregion

        #region Events

        internal event EventHandler<ConnectionStateChangedEventArgs> ConnectionStateChanged;
        internal event EventHandler<ReconnectionTimeChangedEventArgs> ReconnectionTimeChanged;

        #endregion

        #region Constants

        private const int StartReconnectionTime = 5000;
        private const int ReconnectionTimeout = 1000;

        #endregion

    }

    #region EventArgs

    internal class ConnectionStateChangedEventArgs : EventArgs
    {
        internal ConnectionStateChangedEventArgs(bool isConnected)
        {
            IsConnected = isConnected;
        }

        public bool IsConnected { get; }

    }

    internal class ReconnectionTimeChangedEventArgs : EventArgs
    {
        internal ReconnectionTimeChangedEventArgs(int remainingTime)
        {
            RemainingTime = remainingTime;
        }

        public int RemainingTime { get; }

    }

    #endregion

}