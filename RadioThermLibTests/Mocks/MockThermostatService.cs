using RadioThermLib.Models;
using RadioThermLib.Services;

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously

namespace RadioThermLibTests.Mocks
{
    internal class MockThermostatService : IThermostatService
    {
        ThermostatState state;

        public MockThermostatService()
        {
            // we need a mode so we can mock setting the temp. vm will not move it from off to cool or heat
            state = Default.EmptyState with { ThermostatMode = ThermostatModeEnum.Cool };
        }

        public bool ThrowErrorInGetStatus { get; set; } = false;

        public async Task<ThermostatState?> GetStatusAsync(string url)
        {
            if (ThrowErrorInGetStatus)
                throw new TimeoutException("Unit Test Exception");

            return state with { Temperature = 69.0f };
        }

        public async Task<string> GetVersionAsync(string url)
        {
            return "CT69 V6.9";
        }

        public async Task<string> GetUnitNameAsync(string url)
        {
            return "Unit Name - Test";
        }

        public Task<ThermostatProgram?> GetCoolProgram(string url)
        {
            throw new NotImplementedException();
        }

        public Task SetCoolProgram(string url, ThermostatProgram program)
        {
            throw new NotImplementedException();
        }

        public async Task SetCoolAsync(string url, float temp)
        {
            state = state with { TemporaryCoolSetPoint = temp, CurrentState = ThermostatStateEnum.Cool };
        }

        public async Task SetHeatAsync(string url, float temp)
        {
            state = state with { TemporaryHeatSetPoint = temp, CurrentState = ThermostatStateEnum.Heat };
        }

        public ThermostatError? GetError()
        {
            throw new NotImplementedException();
        }
    }
}