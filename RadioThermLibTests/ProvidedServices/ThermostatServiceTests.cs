using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RadioThermLib.ProvidedServices;
using RadioThermLib.Services;
using RadioThermLibTests.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioThermLib.ProvidedServices.Tests
{
    [TestClass()]
    public class ThermostatServiceTests
    {
        [TestMethod()]
        public async Task GetStatusAsyncTest()
        {
            var uut = new ThermostatService();

            var status = await uut.GetStatusAsync();

            Assert.IsNotNull(status);
        }

        [TestMethod()]
        public async Task GetVersionAsyncTest()
        {
            var uut = new ThermostatService();

            var version = await uut.GetVersionAsync();

            Assert.IsNotNull(version);
            Assert.IsFalse(string.IsNullOrWhiteSpace(version));
            
            // probably diff for everyone else
            Assert.AreEqual("CT50 V1.94", version);
            
            Console.WriteLine(version);
        }

        [TestMethod()]
        public async Task SetCoolAsyncTest()
        {
            var uut = new ThermostatService();
            //await uut.SetCoolAsync(72.0f);

            Assert.Fail();
        }

        [TestMethod()]
        public async Task SetHeatAsyncTest()
        {
            var uut = new ThermostatService();

            //await uut.SetHeatAsync(72.0f);

            Assert.Fail();
        }

        [TestInitialize()]
        public void SetupIoc()
        {
            Ioc.Default.ConfigureServices(new ServiceCollection()
                .AddSingleton<ISettingsService, MockSettingsService>()
                .BuildServiceProvider());
        }
    }
}