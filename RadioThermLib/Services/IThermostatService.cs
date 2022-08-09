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
        Task<ThermostatState?> GetStatusAsync(string url);

        Task<string> GetVersionAsync(string url);

        Task<string> GetUnitNameAsync(string url);

        Task<ThermostatProgram?> GetCoolProgram(string url);

        Task SetCoolAsync(string url, float temp);

        Task SetHeatAsync(string url, float temp);

        ThermostatError? GetError();
    }

    public class ThermostatError
    {
        public Type? ExceptionType { get; init; }
        public string? ErrorMessage { get; init; }
        public Exception ExceptionObj { get; set; }
    }
}
