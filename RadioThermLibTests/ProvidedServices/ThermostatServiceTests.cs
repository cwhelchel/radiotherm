using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RadioThermLib.Services;

namespace RadioThermLib.ProvidedServices.Tests
{
    [TestClass()]
    public class ThermostatServiceTests
    {
        [TestMethod()]
        public async Task GetStatusAsyncTest()
        {
            var uut = Ioc.Default.GetService<IThermostatService>();

            var status = await uut!.GetStatusAsync(null!);

            Assert.IsNotNull(status);
            Assert.AreEqual(Models.ThermostatStateEnum.Off, status.CurrentState);
            Assert.AreEqual(Models.ThermostatModeEnum.Cool, status.ThermostatMode);
            Assert.AreEqual(69.0f, status.Temperature);
        }

        [TestMethod()]
        public async Task GetVersionAsyncTest()
        {
            var uut = Ioc.Default.GetService<IThermostatService>();

            var version = await uut!.GetVersionAsync(null!);

            Assert.IsNotNull(version);
            Assert.IsFalse(string.IsNullOrWhiteSpace(version));

            // probably diff for everyone else
            Assert.AreEqual("CT69 V6.9", version);

            Console.WriteLine(version);
        }

        [TestMethod()]
        public async Task SetCoolAsyncTest()
        {
            var uut = Ioc.Default.GetService<IThermostatService>();

            await uut!.SetCoolAsync(null!, 72.0f);

            var status = await uut.GetStatusAsync(null!);
            Assert.IsNotNull(status);
            Assert.AreEqual(Models.ThermostatStateEnum.Cool, status.CurrentState);
            Assert.AreEqual(72.0f, status.TemporaryCoolSetPoint);
        }

        [TestMethod()]
        public async Task SetHeatAsyncTest()
        {
            var uut = Ioc.Default.GetService<IThermostatService>();

            await uut!.SetHeatAsync(null!, 72.0f);

            var status = await uut.GetStatusAsync(null!);
            Assert.IsNotNull(status);
            Assert.AreEqual(Models.ThermostatStateEnum.Heat, status.CurrentState);
            Assert.AreEqual(72.0f, status.TemporaryHeatSetPoint);
        }
    }
}