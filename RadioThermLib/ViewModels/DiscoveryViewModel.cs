using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using RadioThermLib.Services;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Net;
using System.Net.Sockets;

namespace RadioThermLib.ViewModels
{
    public class DiscoveryViewModel : ObservableObject
    {
        private string localIp;
        private bool isUpdating;
        private ObservableCollection<string> discovered;
        private string selectedDevice;
        private readonly ISettingsService settingsService;
        private readonly IViewService viewService;

        public DiscoveryViewModel(ISettingsService settingsService, IViewService viewService)
        {
            discovered = new ObservableCollection<string>();
            StartDiscoveryCommand = new AsyncRelayCommand(StartDiscoveryAsync);
            SelectDeviceCommand = new RelayCommand(SelectDevice);
            this.settingsService = settingsService;
            this.viewService = viewService;
        }

        public string LocalIp
        {
            get => localIp;
            set => SetProperty(ref localIp, value);
        }

        public bool IsUpdating
        {
            get => isUpdating;
            set => SetProperty(ref isUpdating, value);
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

        public IRelayCommand StartDiscoveryCommand { get; }

        public IRelayCommand SelectDeviceCommand { get; }

        public async Task StartDiscoveryAsync()
        {
            var localIp = GetLocalIpAddress();

            if (localIp == null)
                return;

            LocalIp = localIp;
            IsUpdating = true;

            using (var v = new MarvellDiscovery(IPAddress.Parse(localIp), 5000))
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

            IsUpdating = false;
        }

        public void SelectDevice()
        {
            settingsService.SetValue("ThermostatUrl", "http://" + SelectedDevice);
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
}
