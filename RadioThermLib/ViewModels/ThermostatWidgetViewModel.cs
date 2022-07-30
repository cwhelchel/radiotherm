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
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using RadioThermLib.Models;
using RadioThermLib.Services;

namespace RadioThermLib.ViewModels
{
    public class ThermostatWidgetViewModel : ObservableRecipient
    {
        private readonly ISettingsService settingsService;
        private readonly IThermostatService thermostatService;
        private readonly IViewService viewService;
        private ThermostatState? state;
        private ObservableCollection<ThermostatViewModel> thermostatVms;
        private string? unitName;
        private string? version;
        private float currentSetpoint;
        private bool isUpdating;
        private string selectedDevice = "";

        public ThermostatWidgetViewModel(ISettingsService settingsService, IThermostatService thermostatService, IViewService viewService)
        {
            this.settingsService = settingsService;
            this.thermostatService = thermostatService;
            this.viewService = viewService;
            //UpdateCommand = new AsyncRelayCommand<string>(UpdateAsync!);
            //SetTemperatureCommand = new AsyncRelayCommand<string>(SetTemperatureAsync);
            Thermostats = new ObservableCollection<ThermostatViewModel>();
        }

        #region Properties

        public ObservableCollection<ThermostatViewModel> Thermostats
        {
            get => thermostatVms;
            set => SetProperty(ref thermostatVms, value);
        }

       /* public ThermostatState? State
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
        }*/

        public bool IsUpdating
        {
            get => isUpdating;
            set => SetProperty(ref isUpdating, value);
        }

        public IAsyncRelayCommand UpdateCommand { get; }
        public IAsyncRelayCommand<string> SetTemperatureCommand { get; }

        #endregion //Properties 

        ///// <summary>
        ///// Updates the Thermostat's <see cref="State"/>.
        ///// </summary>
        ///// <param name="selectedDeviceIp">The thermostat device's IP address.</param>
        ///// <returns>awaitable task.</returns>
        //public async Task UpdateAsync(string selectedDeviceIp)
        //{
        //    settingsService.SetValue("ThermostatUrl", "http://" + selectedDeviceIp);

        //    IsUpdating = true;

        //    State = await thermostatService.GetStatusAsync();

        //    if (State == null)
        //    {
        //        var error = thermostatService.GetError();
        //        var msg = $"Error Getting Thermostat Status\r\nType: {error!.ExceptionType}\r\n\r\nError: {error!.ErrorMessage}";
        //        this.viewService.ShowDialog("Error Updating", msg);
        //        this.IsUpdating = false;
        //        return;
        //    }

        //    if (State.ThermostatMode == ThermostatModeEnum.Cool)
        //        CurrentSetpoint = State.TemporaryCoolSetPoint;
        //    else if (State.ThermostatMode == ThermostatModeEnum.Heat)
        //        CurrentSetpoint = State.TemporaryHeatSetPoint;

        //    UnitName = await thermostatService.GetUnitNameAsync();

        //    Version = await thermostatService.GetVersionAsync();

        //    IsUpdating = false;
        //}

        ///// <summary>
        ///// Sets the remote thermostat's set point (it's target temperature).
        ///// </summary>
        ///// <param name="newSetPoint">New target temperature. In degrees F</param>
        ///// <returns>awaitable task</returns>
        ///// <exception cref="NullReferenceException">Occurs if this is called before <see cref="UpdateAsync"/></exception>
        //public async Task SetTemperatureAsync(string? newSetPoint)
        //{
        //    IsUpdating = true;

        //    if (float.TryParse(newSetPoint, out float newTemp))
        //    {
        //        if (State.ThermostatMode == ThermostatModeEnum.Cool)
        //        {
        //            await thermostatService.SetCoolAsync(newTemp);
        //        }
        //        else if (State.ThermostatMode == ThermostatModeEnum.Heat)
        //        {
        //            await thermostatService.SetHeatAsync(newTemp);
        //        }

        //        await UpdateAsync(this.selectedDevice);
        //    }

        //    IsUpdating = false;
        //}

        protected override void OnActivated()
        {
            RegisterMessages();
        }

        private void RegisterMessages()
        {
            MessageHandler<ThermostatWidgetViewModel, UpdateRequestMessage> handler = async (r, m) =>
            {
                // run the update and respond true.
                UpdateAllDevices();
                m.Reply(true);
            };

            Messenger.Register(this, handler);
        }

        private async void UpdateAllDevices()
        {
            //this.IsUpdating = true;

            Thermostats.Clear();

            var discovered = settingsService.GetValue<List<string>>("DiscoveredAddresses");
            var manual = settingsService.GetValue<List<string>>("ManualAddresses");

            foreach (var dev in discovered)
            {
                var vm = Ioc.Default.GetService<ThermostatViewModel>();
                Thermostats.Add(vm);
                await vm.UpdateAsync(dev);
            }

            foreach (var dev in manual)
            {
                var vm = Ioc.Default.GetService<ThermostatViewModel>();
                Thermostats.Add(vm);
                await vm.UpdateAsync(dev);
            }

            //this.IsUpdating = false;
        }
    }
}
