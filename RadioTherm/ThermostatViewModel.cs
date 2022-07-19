using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;

namespace RadioTherm
{
    public class ThermostatViewModel : ViewModelBase
    {
        private readonly Thermostat thermostat;
        private float temperature;
        private string unitName;
        private bool temporaryOverride;
        private string version;
        private ThermostatMode thermostatMode;
        private float currentTemperature;
        private bool isUpdating;

        public ThermostatViewModel(Thermostat thermostat)
        {
            this.thermostat = thermostat;
            this.UpdateCommand = new AsyncRelayCommand(UpdateAsync);
        }

        public float Temperature
        {
            get => temperature;
            set => SetProperty(ref temperature, value);
        }

        public string UnitName
        {
            get => unitName;
            set => SetProperty(ref unitName, value);
        }

        public bool TemporaryOverride
        {
            get => temporaryOverride;
            set => SetProperty(ref temporaryOverride, value);
        }

        public string Version
        {
            get => version;
            set => SetProperty(ref version, value);
        }

        public ThermostatMode ThermostatMode
        {
            get => thermostatMode;
            set => SetProperty(ref thermostatMode, value);
        }

        public float CurrentTemperature
        {
            get => currentTemperature;
            set => SetProperty(ref currentTemperature, value);
        }

        public bool IsUpdating
        {
            get => isUpdating;
            set => SetProperty(ref isUpdating, value);
        }

        public IAsyncRelayCommand UpdateCommand { get; }

        public async Task UpdateAsync()
        {
            this.IsUpdating = true;

            await this.thermostat.Update();

            this.Temperature = this.thermostat.Temperature;
            this.CurrentTemperature = this.thermostat.CurrentSetpoint;
            this.UnitName = this.thermostat.UnitName;
            this.TemporaryOverride = this.thermostat.TemporaryOverride;
            this.Version = this.thermostat.Version;
            this.ThermostatMode = this.thermostat.Mode;

            this.IsUpdating = false;
        }
    }
}
