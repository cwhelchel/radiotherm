using RadioThermLib.Services;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioThermLibTests.Services
{
    public class MockSettingsService : ISettingsService
    {
        bool setup = false;
        Dictionary<string, object> values = new Dictionary<string, object>();

        public T? GetValue<T>(string key)
        {
            if (!setup)
            {
                // these values should be changed for future tests
                values["DiscoveryTimeout"] = 1000;
                values["LocalIpAddress"] = "192.168.11.2";
                values["ThermostatUrl"] = "http://192.168.11.156";
                values["ThermostatUrls"] = new StringCollection() { "http://192.168.11.156", "http://192.168.11.157" };
                setup = true;
            }

            return (T)values[key];
        }

        public void SetValue<T>(string key, T? value)
        {
            throw new NotImplementedException();
        }
    }
}
