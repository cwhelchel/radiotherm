using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using RadioThermLib.Models;
using RadioThermLib.Services;

namespace RadioThermLib.ViewModels
{
    public class ThermostatViewModel : ObservableRecipient
    {
        private readonly ISettingsService settingsService;
        private readonly IThermostatService thermostatService;
        private readonly IViewService viewService;
        private ObservableCollection<string> discovered;
        private string? unitName;
        private string? version;
        private float currentSetpoint;
        private bool isUpdating;
        private bool isDiscovering;
        private ThermostatState state;
        private string selectedDevice;

        public ThermostatViewModel(ISettingsService settingsService, IThermostatService thermostatService, IViewService viewService)
        {
            this.settingsService = settingsService;
            this.thermostatService = thermostatService;
            this.viewService = viewService;
            this.discovered = new ObservableCollection<string>();
            UpdateCommand = new AsyncRelayCommand(UpdateAsync);
            SetTemperatureCommand = new AsyncRelayCommand<string>(SetTemperatureAsync);
            StartDiscoveryCommand = new AsyncRelayCommand(StartDiscoveryAsync);
        }

        #region Properties 

        public ThermostatState? State
        {
            get => state; 
            set => SetProperty(ref state, value);
        }

        public string? UnitName
        {
            get => unitName; 
            set => SetProperty(ref unitName, value);
        }

        public float CurrentSetpoint
        {
            get => currentSetpoint;
            set => SetProperty(ref currentSetpoint, value);
        }

        public string? Version
        {
            get => version;
            set => SetProperty(ref version, value);
        }

        public bool IsUpdating
        {
            get => isUpdating;
            set => SetProperty(ref isUpdating, value);
        }

        public bool IsDiscovering
        {
            get => isDiscovering;
            set => SetProperty(ref isDiscovering, value);
        }

        public string SelectedDevice
        {
            get => selectedDevice;
            set => SetProperty(ref selectedDevice, value);
        }

        public ObservableCollection<string> Discovered
        {
            get => discovered;
            set => SetProperty(ref discovered, value);
        }

        public IAsyncRelayCommand StartDiscoveryCommand { get; }
        public IAsyncRelayCommand UpdateCommand { get; }
        public IAsyncRelayCommand<string> SetTemperatureCommand { get; }

        #endregion //Properties 

        public async Task UpdateAsync()
        {
            settingsService.SetValue("ThermostatUrl", "http://" + SelectedDevice);

            IsUpdating = true;

            State = await thermostatService.GetStatusAsync();

            if (State == null)
            {
                var error = thermostatService.GetError();
                var msg = $"Error Getting Thermostat Status\r\nType: {error!.ExceptionType}\r\n\r\nError: {error!.ErrorMessage}";
                this.viewService.ShowDialog("Error Updating", msg);
                this.IsUpdating = false;
                return;
            }

            if (State.ThermostatMode == ThermostatModeEnum.Cool)
                CurrentSetpoint = State.TemporaryCoolSetPoint;
            else if (State.ThermostatMode == ThermostatModeEnum.Heat)
                CurrentSetpoint = State.TemporaryHeatSetPoint;

            UnitName = await thermostatService.GetUnitNameAsync();

            Version = await thermostatService.GetVersionAsync();

            IsUpdating = false;
        }

        public async Task SetTemperatureAsync(string? newSetPoint)
        {
            IsUpdating = true;

            if (float.TryParse(newSetPoint, out float newTemp))
            {
                if (State.ThermostatMode == ThermostatModeEnum.Cool)
                {
                    await thermostatService.SetCoolAsync(newTemp);
                }
                else if (State.ThermostatMode == ThermostatModeEnum.Heat)
                {
                    await thermostatService.SetHeatAsync(newTemp);
                }

                await UpdateAsync();
            }

            IsUpdating = false;
        }

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

        protected override void OnActivated()
        {
            MessageHandler<ThermostatViewModel, UpdateRequestMessage> handler = async (r, m) => {
                // run the update and respond true.
                this.SelectedDevice = m.SelectedDevice;
                await this.UpdateAsync();
                m.Reply(true); 
            };

            Messenger.Register(this, handler);
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
