using RadioThermLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioThermLib.Services
{
    public interface IThermostatService
    {
        Task<ThermostatState?> GetStatusAsync();

        Task<string> GetVersionAsync();

        Task<string> GetUnitNameAsync();

        Task SetCoolAsync(float temp);

        Task SetHeatAsync(float temp);

        ThermostatError? GetError();
    }

    public class ThermostatError
    {
        public Type? ExceptionType { get; init; }
        public string? ErrorMessage { get; init; }
    }
}
