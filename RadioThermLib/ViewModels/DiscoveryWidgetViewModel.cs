
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
using System.Windows.Input;

namespace RadioThermLib.ViewModels
{
    public class DiscoveryWidgetViewModel : ObservableRecipient
    {
        private readonly ISettingsService settingsService;
        private readonly IViewService viewService;
        private bool isDiscovering;
        private bool alreadyDiscovered = false;
        private ObservableCollection<string> discovered;
        private string selectedDevice;

        public DiscoveryWidgetViewModel(ISettingsService settingsService, IViewService viewService)
        {
            this.settingsService = settingsService;
            this.viewService = viewService;
            this.discovered = new ObservableCollection<string>();
            this.selectedDevice = String.Empty;
            StartDiscoveryCommand = new AsyncRelayCommand<bool>(StartDiscoveryAsync);
            RequestUpdateCommand = new RelayCommand(RequestUpdate);
            AddRemoteCommand = new RelayCommand<string>(AddRemote);
            RemoveRemoteCommand = new RelayCommand<string>(RemoveRemote);
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
        public IRelayCommand AddRemoteCommand { get; }
        public IRelayCommand RemoveRemoteCommand { get; }

        public async Task StartDiscoveryAsync(bool force = true)
        {
            int timeout = this.settingsService.GetValue<int>("DiscoveryTimeout");
            var manualEntries = this.settingsService.GetValue<List<string>>("ManualAddresses");

            if (alreadyDiscovered && !force)
                return;

            IsDiscovering = true;

            var localIp = GetLocalIpAddress();

            if (localIp == null)
                return;

            await DiscoverDevices(localIp, timeout);

            var toSave = new List<string>(Discovered);
            this.settingsService.SetValue("DiscoveredAddresses", toSave);

            foreach (var manual in manualEntries!)
            {
                Discovered.Add(manual);
            }

            IsDiscovering = false;

            // set a flag saying we got something
            alreadyDiscovered = Discovered.Count > 0;
        }

        public void RequestUpdate()
        {
            if (string.IsNullOrEmpty(SelectedDevice))
                return;

            var urm = new UpdateRequestMessage() { SelectedDevice = this.SelectedDevice };
            var res = WeakReferenceMessenger.Default.Send(urm);
        }

        private async Task DiscoverDevices(string localIp, int timeout)
        {
            using (var v = new MarvellDiscovery(IPAddress.Parse(localIp), timeout))
            {
                await Task.Run(() => { v.Discover(); });

                foreach (var ip in v.DiscoveredDevices)
                {
                    if (!string.IsNullOrWhiteSpace(ip.ToString()))
                        Discovered.Add(ip.ToString());
                }
            }
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

        private void AddRemote(string? ipToAdd)
        {
            if (!string.IsNullOrWhiteSpace(ipToAdd))
            {
                // TODO: validate IP
                // no, lets not do this. that way we can add hostname and not just ips
                //if (! IPAddress.TryParse(ipToAdd, out IPAddress _))
                //{
                //    if (!Uri.IsWellFormedUriString(ipToAdd, UriKind.Absolute))
                //    {
                //        viewService.ShowDialog("IP parsing error", "Could not parse IP address or Uri");
                //        return;
                //    }
                //}

                var manualEntries = this.settingsService.GetValue<List<string>>("ManualAddresses");

                manualEntries?.Add(ipToAdd);

                this.settingsService.SetValue("ManualAddresses", manualEntries);

                Discovered.Add(ipToAdd);
            }
        }

        private void RemoveRemote(string? ipToRemove)
        {
            if (string.IsNullOrWhiteSpace(ipToRemove))
                return;

            var manualEntries = this.settingsService.GetValue<List<string>>("ManualAddresses");
            manualEntries?.Remove(ipToRemove);
            this.settingsService.SetValue("ManualAddresses", manualEntries);

            this.Discovered.Remove(ipToRemove);
        }
    }

    /// <summary>
    /// Message to request an update.
    /// </summary>
    public sealed class UpdateRequestMessage : RequestMessage<bool>
    {
        public string SelectedDevice { get; init; } = "";
    }
}
