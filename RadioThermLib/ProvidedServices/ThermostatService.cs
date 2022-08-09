using RadioThermLib.Models;
using RadioThermLib.Services;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging;

namespace RadioThermLib.ProvidedServices
{
    public class ThermostatService : IThermostatService
    {
        private readonly ILogger<ThermostatService> log;
        private readonly HttpClient client;
        private ThermostatError? storedError;

        public ThermostatService(ISettingsService settingsService, ILogger<ThermostatService> log)
        {
            this.log = log;
            var msgHandler = settingsService.GetHttpMessageHandler();
            this.client = new HttpClient(msgHandler);
            //client.Timeout = TimeSpan.FromSeconds(10.0);
        }

        /// <inheritdoc/>
        public async Task<ThermostatState?> GetStatusAsync(string url)
        {
            ThermostatState? thermostatState = null;

            try
            {
                var response = await client.GetAsync(url + "/tstat");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                thermostatState = JsonSerializer.Deserialize<ThermostatState>(json);
            }
            catch (Exception ex)
            {
                this.log.LogError(ex, $"exception caught while getting thermostat status at {url}");
                thermostatState = null;
                storedError = new ThermostatError { ErrorMessage = ex.Message, ExceptionType = ex.GetType(), ExceptionObj = ex };
            }

            return thermostatState;
        }

        /// <inheritdoc/>
        public async Task<string> GetUnitNameAsync(string url)
        {
            var msg = await GetString(url, "/sys/name", "name");

            return msg;
        }

        /// <inheritdoc/>
        public async Task<string> GetVersionAsync(string url)
        {
            var msg = await GetString(url, "/tstat/model", "model");

            return msg;
        }

        /// <inheridoc />
        public async Task<ThermostatProgram?> GetCoolProgram(string url)
        {
            ThermostatProgram? program = null;
            try
            {
                var json = await GetJson(url, "/tstat/program/cool");
                program = JsonSerializer.Deserialize<ThermostatProgram>(json);
            }
            catch (Exception ex)
            {
                this.log.LogError(ex, $"exception caught while getting thermostat Cool program at {url}");

                program = null;
                storedError = new ThermostatError { ErrorMessage = ex.Message, ExceptionType = ex.GetType(), ExceptionObj = ex };
            }

            log.LogDebug($"got cool program: {program}");

            return program;
        }

        /// <inheritdoc/>
        public async Task SetCoolAsync(string url, float temp)
        {
            if (temp < 35.0f || temp > 95.0f)
                throw new ArgumentOutOfRangeException("temp", temp, strings.ValidTemperatureRangeInputMsg);


            var jsonObj = new JsonObject
            {
                { "t_cool", temp }
            };

            var sc = new StringContent(jsonObj.ToJsonString(), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url + "/tstat", sc);

            response.EnsureSuccessStatusCode();
        }

        /// <inheritdoc/>
        public async Task SetHeatAsync(string url, float temp)
        {
            if (temp < 35.0f || temp > 95.0f)
                throw new ArgumentOutOfRangeException("temp", temp, strings.ValidTemperatureRangeInputMsg);

            var jsonObj = new JsonObject
            {
                { "t_heat", temp }
            };

            var sc = new StringContent(jsonObj.ToJsonString(), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url + "/tstat", sc);

            response.EnsureSuccessStatusCode();
        }

        /// <inheritdoc/>
        public ThermostatError? GetError()
        {
            var copy = this.storedError;
            this.storedError = null;
            return copy;
        }

        private async Task<string> GetString(string url, string endpoint, string jsonPropertyName)
        {
            string json = await GetJson(url, endpoint);

            JsonNode? jn = JsonNode.Parse(json);

            return jn?[jsonPropertyName]!.GetValue<string>().Trim() ?? string.Empty;
        }

        private async Task<string> GetJson(string url, string endpoint)
        {
            var response = await client.GetAsync(url + endpoint);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
