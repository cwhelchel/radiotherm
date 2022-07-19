using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RadioThermLib.Services;
using RadioThermLib.ViewModels;
using RadioThermLibTests.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioThermLibTests.ViewModels
{
    [TestClass()]
    public class ThermostatViewModelTests
    {
        [TestMethod()]
        public void ThermostatViewModelTest()
        {
            var vm = Ioc.Default.GetService<ThermostatViewModel>();
            Assert.IsNotNull(vm);
            Assert.IsNotNull(vm.UpdateCommand);
            Assert.IsNotNull(vm.SetTemperatureCommand);
            Assert.IsNotNull(vm.StartDiscoveryCommand);
        }

        [TestMethod()]
        public async Task UpdateAsyncTest()
        {
            var vm = Ioc.Default.GetService<ThermostatViewModel>();
            Assert.IsNotNull(vm);

            await vm.UpdateAsync();

            Assert.IsNotNull(vm.State);
            Assert.AreEqual(0.0f, vm.State.Temperature);
        }

        [TestMethod()]
        public async Task SetTemperatureAsyncTest()
        {
            var vm = Ioc.Default.GetService<ThermostatViewModel>();
            
            await vm.UpdateAsync();
            Assert.AreEqual(0.0f, vm.State.TemporaryCoolSetPoint);

            await vm.SetTemperatureAsync("69.0");

            Assert.AreEqual(69.0f, vm.State.TemporaryCoolSetPoint);
        }

        [TestMethod()]
        public async Task StartDiscoveryAsyncTest()
        {
            var vm = Ioc.Default.GetService<ThermostatViewModel>();

            await vm.UpdateAsync();
            await vm.StartDiscoveryAsync();
        }

        [TestInitialize()]
        public void InitializeTest()
        {
            Ioc.Default.ConfigureServices(new ServiceCollection()
                 .AddSingleton<ISettingsService, MockSettingsService>()
                 .AddSingleton<IThermostatService, MockThermostatService>()
                 .AddTransient<ThermostatViewModel>()
                 .BuildServiceProvider());
        }
    }
}