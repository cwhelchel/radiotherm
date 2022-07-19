using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace RadioTherm
{
    public class Thermostat
    {
        readonly HttpClient client = new HttpClient(new HttpClientHandler());
        readonly string url = "";

        public Thermostat(string url) 
        {
            this.url = url;
        }

        public ThermostatObj ThermostatObj { get; private set; }

        public string Version { get; private set; }

        public string ThermostatJson { get; private set; }

        public string UnitName { get; private set; }

        public bool TemporaryOverride { get; private set; }

        public float Temperature { get; private set; }

        public float CurrentSetpoint { get; private set; }

        public ThermostatMode Mode { get; private set; }

        public async Task Update()
        {
            var response = await client.GetAsync(url + "/tstat/");

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            this.ThermostatObj = JsonSerializer.Deserialize<ThermostatObj>(json);
            this.ThermostatJson = json;
            this.TemporaryOverride = ThermostatObj._override == 1;
            this.Temperature = ThermostatObj.temp;
            this.Mode = ThermostatObj.tmode;

            this.CurrentSetpoint = (Mode == ThermostatMode.Cool) ? ThermostatObj.t_cool : ThermostatObj.t_heat;

            this.Version = await GetVersion();
            this.UnitName = await GetString("/sys/name", "name");
        }

        public async Task<string> GetVersion()
        {
            var msg = await GetString("/tstat/model", "model");

            return msg;
        }

        public async Task<string> GetSystemInfo()
        {
            string sss = "";

            var response = await client.GetAsync(url + "/sys/services");

            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();

            using (JsonDocument jn = JsonDocument.Parse(json))
            {
                var root = jn.RootElement;
                var x = root.GetProperty("httpd_handlers");

                foreach (var element in x.EnumerateObject())
                {
                    sss += $"{element.Name} - {element.Value}\r\n";
                }
            }

            return sss;
        }

        public async Task<string> GetRawJson(string endpoint)
        {
            var response = await client.GetAsync(url + endpoint);

            response.EnsureSuccessStatusCode();

            string json = await response.Content.ReadAsStringAsync();

            return json;
        }

        public async void SetCool(float temp)
        {
            if (temp < 35.0f || temp > 95.0f)
                throw new ArgumentOutOfRangeException("temp", temp, "valid temperature range input is 35-95 F");

            var jsonObj = new JsonObject();
            jsonObj.Add("t_cool", temp);

            var sc = new StringContent(jsonObj.ToJsonString(), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url + "/tstat", sc);

            response.EnsureSuccessStatusCode();
        }

        public async void SetHeat(float temp)
        {
            if (temp < 35.0f || temp > 95.0f)
                throw new ArgumentOutOfRangeException("temp", temp, "valid temperature range input is 35-95 F");

            var jsonObj = new JsonObject();
            jsonObj.Add("t_heat", temp);

            var sc = new StringContent(jsonObj.ToJsonString(), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url + "/tstat", sc);

            response.EnsureSuccessStatusCode();
        }

        public override string ToString()
        {
            string temp = $"{UnitName} ({Version}) is set to {ThermostatObj.SetPointState} {ThermostatObj.tmode} at {ThermostatObj.t_cool} degrees.\r\n";
            //return temp;
            return $"ThermostatJson: {ThermostatJson}";
        }

        private async Task<string> GetString(string endpoint, string jsonPropertyName)
        {
            string json = await GetJson(endpoint);

            JsonNode jn = JsonNode.Parse(json);

            return jn[jsonPropertyName].GetValue<string>().Trim();
        }

        private async Task<string> GetJson(string endpoint)
        {
            var response = await client.GetAsync(url + endpoint);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
