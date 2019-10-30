using System;
using Avalonia.Utilities;
using ReactiveUI;

namespace RC.Client.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            DeviceInfo = new DeviceInfoViewModel();

            IsConnected = Application.Instance.IsConnected;
            ReconnectionTime = Application.Instance.ReconnectionTime;
            SubscribeToApplicationEvents();
        }

        #region Private Methods

        private void SubscribeToApplicationEvents()
        {
            WeakEventHandlerManager.Subscribe<ClientApplication, ConnectionStateChangedEventArgs, MainViewModel>(
                Application.Instance, nameof(Application.Instance.ConnectionStateChanged), OnConnectionStateChanged);
            WeakEventHandlerManager.Subscribe<ClientApplication, ReconnectionTimeChangedEventArgs, MainViewModel>(
                Application.Instance, nameof(Application.Instance.ReconnectionTimeChanged), OnReconnectionTimeChanged);
        }

        private void OnConnectionStateChanged(object sender, ConnectionStateChangedEventArgs e)
        {
            IsConnected = e.IsConnected;
        }

        private void OnReconnectionTimeChanged(object sender, ReconnectionTimeChangedEventArgs e)
        {
            ReconnectionTime = e.RemainingTime / 1000;
        }

        #endregion

        #region Properties

        public DeviceInfoViewModel DeviceInfo { get; }

        #endregion

        #region Reactive Properties
        public bool IsConnected
        {
            get => _isConnected;
            private set
            {
                _isConnected = value;
                this.RaisePropertyChanged();
            }
        }

        public int ReconnectionTime
        {
            get => _reconnectionTime;
            private set
            {
                _reconnectionTime = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion

        #region Private Fields

        private bool _isConnected;
        private int _reconnectionTime;

        #endregion

    }

}
