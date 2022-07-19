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
            Assert.AreEqual(Models.ThermostatStateEnum.Off, status.CurrentState);
            Assert.AreEqual(69.0f, status.Temperature);
            Assert.AreEqual(666.999f, status.TemporaryHeatSetPoint);
        }

        [TestMethod()]
        public async Task GetVersionAsyncTest()
        {
            var uut = new ThermostatService();

            var version = await uut.GetVersionAsync();

            Assert.IsNotNull(version);
            Assert.IsFalse(string.IsNullOrWhiteSpace(version));
            
            // probably diff for everyone else
            Assert.AreEqual("CT69 V6.9", version);
            
            Console.WriteLine(version);
        }

        [TestMethod()]
        public async Task SetCoolAsyncTest()
        {
            var uut = new ThermostatService();

            await uut.SetCoolAsync(72.0f);

            var status = await uut.GetStatusAsync();
            Assert.IsNotNull(status);
            Assert.AreEqual(Models.ThermostatStateEnum.Cool, status.CurrentState);
            Assert.AreEqual(72.0f, status.TemporaryCoolSetPoint);
        }

        [TestMethod()]
        public async Task SetHeatAsyncTest()
        {
            var uut = new ThermostatService();

            await uut.SetHeatAsync(72.0f);

            var status = await uut.GetStatusAsync();
            Assert.IsNotNull(status);
            Assert.AreEqual(Models.ThermostatStateEnum.Heat, status.CurrentState);
            Assert.AreEqual(72.0f, status.TemporaryHeatSetPoint);
        }
    }
}