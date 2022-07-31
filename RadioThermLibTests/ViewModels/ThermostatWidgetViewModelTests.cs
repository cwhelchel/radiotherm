using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RadioThermLib.Services;
using RadioThermLib.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadioThermLibTests.ViewModels
{
    [TestClass()]
    public class ThermostatWidgetViewModelTests
    {
        [TestMethod()]
        public void ThermostatViewModelTest()
        {
            var vm = Ioc.Default.GetRequiredService<ThermostatWidgetViewModel>();
            Assert.IsNotNull(vm);
        }

        //[TestMethod()]
        //public async Task UpdateAsyncTest()
        //{
        //    var vm = Ioc.Default.GetRequiredService<ThermostatWidgetViewModel>();
        //    Assert.IsNotNull(vm);

        //    await vm.UpdateAsync(null!);

        //    Assert.IsNotNull(vm.State);
        //    Assert.AreEqual(69.0f, vm.State.Temperature);
        //}

        //[TestMethod()]
        //public async Task SetTemperatureAsyncTest()
        //{
        //    var vm = Ioc.Default.GetRequiredService<ThermostatWidgetViewModel>();

        //    // called before updated
        //    await Assert.ThrowsExceptionAsync<NullReferenceException>(async () => await vm.SetTemperatureAsync("69.0"));

        //    await vm.UpdateAsync(null!);
        //    Assert.AreEqual(0.0f, vm.State.TemporaryCoolSetPoint);
        //    await vm.SetTemperatureAsync("69.0");
        //    Assert.AreEqual(69.0f, vm.State.TemporaryCoolSetPoint);
        //}
    }
}