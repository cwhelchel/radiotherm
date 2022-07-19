using RadioThermLib.Services;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace RadioThermWpf.Services
{
    public class JsonSettingService : ISettingsService
    {
        private JsonObject? appSettings;

        public JsonSettingService()
        {
            using (StreamReader r = new StreamReader("settings.json"))
            {
                string json = r.ReadToEnd();
                appSettings = JsonSerializer.Deserialize<JsonObject>(json);
            }
        }

        public T? GetValue<T>(string key)
        {
            return appSettings[key].GetValue<T>();
        }

        public void SetValue<T>(string key, T? value)
        {
            appSettings[key] = value.ToString();
            Save();
        }

        private void Save()
        {
            using (var writer = new FileStream("settings.json", FileMode.Create))
            {
                using (var utfWriter = new Utf8JsonWriter(writer))
                    appSettings.WriteTo(utfWriter);
            }
        }
    }

    public sealed record AppSettings(
        [property: JsonPropertyName("ThermostatUrl")] string ThermostatUrl,
        [property: JsonPropertyName("DiscoveryTimeout")] int DiscoveryTimeout
    );
}
