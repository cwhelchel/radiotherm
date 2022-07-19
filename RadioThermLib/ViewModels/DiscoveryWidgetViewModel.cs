
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using RadioThermLib.Services;

namespace RadioThermLib.ViewModels
{
    public class DiscoveryWidgetViewModel : ObservableRecipient
    {
        private readonly ISettingsService settingsService;
        private bool isDiscovering;
        private ObservableCollection<string> discovered;
        private string selectedDevice;

        public DiscoveryWidgetViewModel(ISettingsService settingsService)
        {
            this.settingsService = settingsService;
            this.discovered = new ObservableCollection<string>();
            this.selectedDevice = String.Empty;
            StartDiscoveryCommand = new AsyncRelayCommand(StartDiscoveryAsync);
            RequestUpdateCommand = new RelayCommand(RequestUpdate);
        }

        public bool IsDiscovering
        {
            get => isDiscovering;
            set => SetProperty(ref isDiscovering, value);
        }

        public ObservableCollection<string> Discovered
        {
            get => discovered;
            set => SetProperty(ref discovered, value);
        }

        public string SelectedDevice
        {
            get => selectedDevice;
            set => SetProperty(ref selectedDevice, value);
        }

        public IAsyncRelayCommand StartDiscoveryCommand { get; }
        public IRelayCommand RequestUpdateCommand { get; }

        public async Task StartDiscoveryAsync()
        {
            int timeout = this.settingsService.GetValue<int>("DiscoveryTimeout");

            IsDiscovering = true;

            var localIp = GetLocalIpAddress();

            if (localIp == null)
                return;

            using (var v = new MarvellDiscovery(IPAddress.Parse(localIp), timeout))
            {
                await Task.Run(() =>
                {
                    v.Discover();
                });

                foreach (var ip in v.DiscoveredDevices)
                {
                    Discovered.Add(ip.ToString());
                }
            }

            IsDiscovering = false;
        }

        public void RequestUpdate()
        {
            var urm = new UpdateRequestMessage() {  SelectedDevice = this.SelectedDevice };
            var res = WeakReferenceMessenger.Default.Send(urm);
        }

        private static string? GetLocalIpAddress()
        {
            // https://stackoverflow.com/a/27376368 

            string? localIP;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                var endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint?.Address.ToString();

                return localIP;
            }
        }
    }

    /// <summary>
    /// Message to request an update.
    /// </summary>
    public sealed class UpdateRequestMessage : RequestMessage<bool> {
        public string SelectedDevice { get; init; } = "";
    }
}
