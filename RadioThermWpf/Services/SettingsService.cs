using RadioThermLib.Services;
using System.Net.Http;

namespace RadioThermWpf.Services
{
    public class SettingsService : ISettingsService
    {
        public HttpMessageHandler GetHttpMessageHandler() => new HttpClientHandler();

        public T? GetValue<T>(string key)
        {
            T val = (T)Properties.Settings.Default[key];
            return val;
        }

        public void SetValue<T>(string key, T? value)
        {
            Properties.Settings.Default[key] = value;

            Properties.Settings.Default.Save();
        }
    }

}
