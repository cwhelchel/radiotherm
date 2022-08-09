using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using RadioThermLib.Models;
using RadioThermLib.Services;

namespace RadioThermLib.ViewModels
{
    public class ThermostatViewModel : ObservableRecipient
    {
        private readonly IThermostatService thermostatService;
        private readonly IViewService viewService;
        private readonly ILogger<ThermostatViewModel> log;
        private string? unitName;
        private float currentSetPoint;
        private bool isUpdating;
        private ThermostatState? state;
        private ProgramViewModel? program;
        private string? version;
        private string? thermostatIp;
        private string? thermostatUrl;
        private bool hasError;

        public ThermostatViewModel(IThermostatService thermostatService, IViewService viewService, ILogger<ThermostatViewModel> log)
        {
            this.thermostatService = thermostatService;
            this.viewService = viewService;
            this.log = log;

            UpdateCommand = new AsyncRelayCommand<string>(UpdateAsync!);
            SetTemperatureCommand = new AsyncRelayCommand<string>(SetTemperatureAsync);
            ShowDetailsCommand = new RelayCommand(ShowDetails);
            GetProgramCommand = new AsyncRelayCommand(GetProgramAsync);
        }

        public IRelayCommand<string> SetTemperatureCommand { get; set; }

        public IRelayCommand<string> UpdateCommand { get; set; }

        public IRelayCommand ShowDetailsCommand { get; set; }

        public IRelayCommand GetProgramCommand { get; set; }

        public ThermostatState? State
        {
            get => state;
            set => SetProperty(ref state, value);
        }

        public ProgramViewModel? Program
        {
            get => program;
            set => SetProperty(ref program, value);
        }

        public bool IsUpdating
        {
            get => isUpdating;
            set => SetProperty(ref isUpdating, value);
        }

        public float CurrentSetPoint
        {
            get => currentSetPoint;
            set => SetProperty(ref currentSetPoint, value);
        }

        public string? UnitName
        {
            get => unitName;
            set => SetProperty(ref unitName, value);
        }

        public string? Version
        {
            get => version;
            set => SetProperty(ref version, value);
        }

        public string? ThermostatIp
        {
            get => thermostatIp;
            set => SetProperty(ref thermostatIp, value);
        }

        public bool HasError
        {
            get => hasError;
            set => SetProperty(ref hasError, value);
        }

        /// <summary>
        /// Updates the Thermostat's <see cref="State"/>.
        /// </summary>
        /// <param name="selectedDeviceIp">The thermostat device's IP address.</param>
        /// <returns>awaitable task.</returns>
        public async Task UpdateAsync(string selectedDeviceIp)
        {
            this.ThermostatIp = selectedDeviceIp;
            this.thermostatUrl = "http://" + selectedDeviceIp;

            this.log.LogInformation($"fetching data from {thermostatUrl}");

            IsUpdating = true;

            try
            {
                bool ok = await FetchData();

                this.HasError = !ok;
            }
            catch (Exception ex)
            {
                this.HasError = true;
                ShowError(ex);
                this.log.LogError(ex, "Error fetching data");
            }

            IsUpdating = false;
        }


        /// <summary>
        /// Sets the remote thermostat's set point (it's target temperature).
        /// </summary>
        /// <param name="newSetPoint">New target temperature. In degrees F</param>
        /// <returns>awaitable task</returns>
        /// <exception cref="NullReferenceException">Occurs if this is called before <see cref="UpdateAsync"/></exception>
        public async Task SetTemperatureAsync(string? newSetPoint)
        {
            // escape conditions
            if (this.thermostatUrl == null || State == null)
                return;

            IsUpdating = true;

            if (float.TryParse(newSetPoint, out float newTemp))
            {
                if (State.ThermostatMode == ThermostatModeEnum.Cool)
                {
                    await thermostatService.SetCoolAsync(this.thermostatUrl, newTemp);
                }
                else if (State.ThermostatMode == ThermostatModeEnum.Heat)
                {
                    await thermostatService.SetHeatAsync(this.thermostatUrl, newTemp);
                }

                await FetchData();
            }

            IsUpdating = false;
        }

        public void ShowDetails()
        {
            this.viewService.ShowThermostatDetails(this);
        }

        public async Task GetProgramAsync()
        {
            if (this.thermostatUrl == null || State == null)
                return;

            IsUpdating = true;

            if (State.ThermostatMode == ThermostatModeEnum.Cool)
            {
                var tsp = await thermostatService.GetCoolProgram(this.thermostatUrl);
                if (tsp != null) 
                    Program = new ProgramViewModel(tsp);
                else
                    log.LogWarning("GetCoolProgram returned null instead of data");
            }
            else if (State.ThermostatMode == ThermostatModeEnum.Heat)
            {
                //await thermostatService.SetHeatAsync(this.thermostatUrl);
            }

            IsUpdating = false;
        }

        private async Task<bool> FetchData()
        {
            if (this.thermostatUrl == null)
                return false;

            var t1 = thermostatService.GetUnitNameAsync(this.thermostatUrl);
            var t2 = thermostatService.GetVersionAsync(this.thermostatUrl);
            var t3 = thermostatService.GetStatusAsync(this.thermostatUrl);

            await Task.WhenAll(t1, t2, t3);

            UnitName = t1.Result;
            Version = t2.Result;
            State = t3.Result;

            if (State == null)
            {
                ShowServiceError();
                return false;
            }

            if (State.ThermostatMode == ThermostatModeEnum.Cool)
                CurrentSetPoint = State.TemporaryCoolSetPoint;
            else if (State.ThermostatMode == ThermostatModeEnum.Heat)
                CurrentSetPoint = State.TemporaryHeatSetPoint;

            return true;
        }

        private void ShowServiceError()
        {
            var error = thermostatService.GetError();
            Debug.Assert(error != null, nameof(error) + " != null");

            this.log.LogError(error.ExceptionObj, $"service error: {error.ErrorMessage}");
            ShowError(error.ExceptionObj);
        }

        private void ShowError(Exception error)
        {
            var msg =
                $"Error Getting Thermostat Status\r\nType: {error.GetType()}\r\n\r\nError: {error.Message}";
            this.viewService.ShowDialog("Error Updating", msg);
        }
    }
}
