using RadioThermLib.Models;
using RadioThermLib.Services;

namespace RadioThermLibTests.Services
{
    internal class MockThermostatService : IThermostatService
    {
        ThermostatState state;
        public MockThermostatService()
        {
            // we need a mode so we can mock setting the temp. vm will not move it from off to cool or heat
            state = Default.EmptyState with { ThermostatMode = ThermostatModeEnum.Cool };
        }

        public ThermostatError? GetError()
        {
            throw new NotImplementedException();
        }

        public async Task<ThermostatState> GetStatusAsync()
        {
            return state;
        }

        public async Task<string> GetUnitNameAsync()
        {
            return "Unit Name Test";
        }

        public async Task<string> GetVersionAsync()
        {
            return "UT 6.9";
        }

        public async Task SetCoolAsync(float temp)
        {
            state = state with { TemporaryCoolSetPoint = temp };
        }

        public async Task SetHeatAsync(float temp)
        {
            state = state with { TemporaryHeatSetPoint = temp };
        }
    }
}