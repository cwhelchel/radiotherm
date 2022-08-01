using RadioThermLib.ProvidedServices;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RadioThermLib.Services;
using RadioThermLibTests.Mocks;

namespace RadioThermLib.ProvidedServices.Tests
{
    [TestClass()]
    public class ThermostatServiceTests
    {
        private const string TestUrl = "http://notreallyreal";

        [TestMethod()]
        public async Task GetStatusAsyncTest()
        {
            var settings = Ioc.Default.GetRequiredService<ISettingsService>();
            var uut = new ThermostatService(settings);
            
            var status = await uut!.GetStatusAsync(TestUrl);

            Assert.IsNotNull(status);
            Assert.AreEqual(Models.ThermostatStateEnum.Off, status.CurrentState);
            Assert.AreEqual(Models.ThermostatModeEnum.Cool, status.ThermostatMode);
            Assert.AreEqual(69.0f, status.Temperature);
        }

        [TestMethod()]
        public async Task GetVersionAsyncTest()
        {
            var settings = Ioc.Default.GetRequiredService<ISettingsService>();
            var uut = new ThermostatService(settings);

            var version = await uut!.GetVersionAsync(TestUrl);

            Assert.IsNotNull(version);
            Assert.IsFalse(string.IsNullOrWhiteSpace(version));

            // probably diff for everyone else
            Assert.AreEqual("CT69 V6.9", version);

            Console.WriteLine(version);
        }

        [TestMethod()]
        public async Task SetCoolAsyncTest()
        {
            var settings = Ioc.Default.GetRequiredService<ISettingsService>();
            var uut = new ThermostatService(settings);

            await uut!.SetCoolAsync(TestUrl, 72.0f);

            var status = await uut.GetStatusAsync(TestUrl);
            Assert.IsNotNull(status);
            Assert.AreEqual(Models.ThermostatStateEnum.Cool, status.CurrentState);
            Assert.AreEqual(72.0f, status.TemporaryCoolSetPoint);
        }

        [TestMethod()]
        public async Task SetHeatAsyncTest()
        {
            var settings = Ioc.Default.GetRequiredService<ISettingsService>();
            var uut = new ThermostatService(settings);

            await uut!.SetHeatAsync(TestUrl, 72.0f);

            var status = await uut.GetStatusAsync(TestUrl);
            Assert.IsNotNull(status);
            Assert.AreEqual(Models.ThermostatStateEnum.Heat, status.CurrentState);
            Assert.AreEqual(72.0f, status.TemporaryHeatSetPoint);
        }

        [TestMethod()]
        public void GetErrorTest()
        {
            var settings = Ioc.Default.GetRequiredService<ISettingsService>();
            var http = settings.GetHttpMessageHandler() as MockHttpMessageHandler;
            http!.ThrowError = true;
            
            var uut = new ThermostatService(settings);

            Assert.ThrowsExceptionAsync<MockException>(() => uut.GetStatusAsync(TestUrl));

            var error = uut!.GetError();

            Assert.IsNotNull(error);
            Assert.IsTrue(error.ExceptionObj is MockException);
            Assert.IsFalse(string.IsNullOrWhiteSpace(error.ErrorMessage));

            http!.ThrowError = false;
        }
    }
}