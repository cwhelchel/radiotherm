using RadioThermLib.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
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

        public HttpMessageHandler GetHttpMessageHandler() => new HttpClientHandler();

        public T? GetValue<T>(string key)
        {
            if (appSettings?[key] is JsonValue)
                return appSettings[key]!.GetValue<T>();
            else if (appSettings?[key] is JsonArray)
            {
                var arr = appSettings[key]?.AsArray();
                return arr.Deserialize<T>();
            }

            throw new KeyNotFoundException();
        }

        public void SetValue<T>(string key, T value)
        {
            if (appSettings != null) 
                appSettings[key] = JsonSerializer.SerializeToNode<T>(value);
            Save();
        }

        private void Save()
        {
            using var writer = new FileStream("settings.json", FileMode.Create);
            using var utfWriter = new Utf8JsonWriter(writer);
            
            appSettings?.WriteTo(utfWriter, new JsonSerializerOptions() {WriteIndented = true});
        }
    }
}
