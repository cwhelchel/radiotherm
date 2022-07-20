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
        private bool initialized = false;

        [TestMethod()]
        public void ThermostatViewModelTest()
        {
            var vm = Ioc.Default.GetService<ThermostatWidgetViewModel>();
            Assert.IsNotNull(vm);
            Assert.IsNotNull(vm.UpdateCommand);
            Assert.IsNotNull(vm.SetTemperatureCommand);
        }

        [TestMethod()]
        public async Task UpdateAsyncTest()
        {
            var vm = Ioc.Default.GetService<ThermostatWidgetViewModel>();
            Assert.IsNotNull(vm);

            await vm.UpdateAsync();

            Assert.IsNotNull(vm.State);
            Assert.AreEqual(0.0f, vm.State.Temperature);
        }

        [TestMethod()]
        public async Task SetTemperatureAsyncTest()
        {
            var vm = Ioc.Default.GetService<ThermostatWidgetViewModel>();
            
            await vm.UpdateAsync();
            Assert.AreEqual(0.0f, vm.State.TemporaryCoolSetPoint);

            await vm.SetTemperatureAsync("69.0");

            Assert.AreEqual(69.0f, vm.State.TemporaryCoolSetPoint);
        }
    }
}