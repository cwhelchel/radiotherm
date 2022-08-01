using System.Collections.Specialized;
using RadioThermLib.Services;

namespace RadioThermLibTests.Mocks
{
    public class MockSettingsService : ISettingsService
    {
        bool setup = false;
        Dictionary<string, object> values = new Dictionary<string, object>();
        MockHttpMessageHandler mockHttpMessageHandler = new MockHttpMessageHandler();

        public HttpMessageHandler GetHttpMessageHandler() => mockHttpMessageHandler;

        public T? GetValue<T>(string key)
        {
            if (!setup)
            {
                // these values should be changed for future tests
                values["DiscoveryTimeout"] = 1000;
                values["LocalIpAddress"] = "192.168.11.2";
                values["ThermostatUrls"] = new StringCollection() { "http://192.168.11.156", "http://192.168.11.157" };
                setup = true;
            }

            return (T)values[key];
        }

        public void SetValue<T>(string key, T value)
        {
            values[key] = value!;
        }
    }
}
