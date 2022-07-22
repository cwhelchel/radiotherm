﻿using CommunityToolkit.Mvvm.DependencyInjection;
using RadioThermLib.Models;
using RadioThermLib.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace RadioThermLib.ProvidedServices
{
    public class ThermostatService : IThermostatService
    {
        private readonly HttpClient client;
        private string? url;
        private ISettingsService settingsService;
        private ThermostatError? storedError;

        public ThermostatService(ISettingsService settinsService)
        {
            this.settingsService = settinsService; // Ioc.Default.GetRequiredService<ISettingsService>();

            var msgHandler = settingsService.GetHttpMessageHandler();
            this.client = new HttpClient(msgHandler);
            //client.Timeout = TimeSpan.FromSeconds(10.0);
        }

        /// <inheritdoc/>
        public async Task<ThermostatState?> GetStatusAsync()
        {
            this.url = settingsService.GetValue<string>("ThermostatUrl");
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
                thermostatState = null;
                storedError = new ThermostatError { ErrorMessage = ex.Message, ExceptionType = ex.GetType() };
            }

            return thermostatState;
        }

        public async Task<string> GetUnitNameAsync()
        {
            this.url = settingsService.GetValue<string>("ThermostatUrl");

            var msg = await GetString(url, "/sys/name", "name");

            return msg;
        }

        /// <inheritdoc/>
        public async Task<string> GetVersionAsync()
        {
            this.url = settingsService.GetValue<string>("ThermostatUrl");

            var msg = await GetString(url, "/tstat/model", "model");

            return msg;
        }

        /// <inheritdoc/>
        public async Task SetCoolAsync(float temp)
        {
            if (temp < 35.0f || temp > 95.0f)
                throw new ArgumentOutOfRangeException("temp", temp, "valid temperature range input is 35-95 F");

            this.url = settingsService.GetValue<string>("ThermostatUrl");


            var jsonObj = new JsonObject
            {
                { "t_cool", temp }
            };

            var sc = new StringContent(jsonObj.ToJsonString(), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url + "/tstat", sc);

            response.EnsureSuccessStatusCode();
        }

        /// <inheritdoc/>
        public async Task SetHeatAsync(float temp)
        {
            if (temp < 35.0f || temp > 95.0f)
                throw new ArgumentOutOfRangeException("temp", temp, "valid temperature range input is 35-95 F");

            this.url = settingsService.GetValue<string>("ThermostatUrl");

            var jsonObj = new JsonObject
            {
                { "t_heat", temp }
            };

            var sc = new StringContent(jsonObj.ToJsonString(), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url + "/tstat", sc);

            response.EnsureSuccessStatusCode();
        }

        public ThermostatError? GetError()
        {
            var copy = this.storedError;
            this.storedError = new ThermostatError();
            return copy;
        }

        private async Task<string> GetString(string url, string endpoint, string jsonPropertyName)
        {
            string json = await GetJson(url, endpoint);

            JsonNode jn = JsonNode.Parse(json);

            return jn[jsonPropertyName].GetValue<string>().Trim();
        }

        private async Task<string> GetJson(string url, string endpoint)
        {
            var response = await client.GetAsync(url + endpoint);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
