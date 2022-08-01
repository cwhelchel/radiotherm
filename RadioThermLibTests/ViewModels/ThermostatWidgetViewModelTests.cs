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
using CommunityToolkit.Mvvm.Messaging;
using RadioThermLibTests.Mocks;

namespace RadioThermLibTests.ViewModels
{
    [TestClass()]
    public class ThermostatWidgetViewModelTests
    {
        [TestMethod()]
        public void ThermostatViewModelTest()
        {
            var vs = Ioc.Default.GetRequiredService<IViewService>() as MockViewService;
            var st = Ioc.Default.GetRequiredService<ISettingsService>() as MockSettingsService;
            var vm = Ioc.Default.GetRequiredService<ThermostatWidgetViewModel>();
            Assert.IsNotNull(vm);


            Assert.AreEqual("", vs.SavedTitle);
            Assert.AreEqual("", vs.SavedMsg);

            // this is the main "entry point" for this class
            // it will register this vm for messages and then waits for an update all
            vm.IsActive = true;

            Assert.IsFalse(vm.IsUpdating);

            // send a msg so the VM updates it's devices.
            var urm = new UpdateRequestMessage() { SelectedDevice = "notreallyrealy" };
            var res = WeakReferenceMessenger.Default.Send(urm);

            // whoops we have no devices in the settings.
            Assert.AreEqual(RadioThermLib.strings.ErrUpdatingTitle, vs.SavedTitle);
            Assert.IsTrue(vs.SavedMsg.StartsWith(RadioThermLib.strings.ErrUpdatingMsg));
            Assert.IsFalse(vm.IsUpdating);

            st.SetValue("DiscoveredAddresses",new List<string> {"192.168.11.156"});
            st.SetValue("ManualAddresses", new List<string> { "" });

            // send a msg so the VM updates it's devices.
            urm = new UpdateRequestMessage() { SelectedDevice = "notreallyrealy" };
            res = WeakReferenceMessenger.Default.Send(urm);

            Assert.IsTrue(res.HasReceivedResponse);
            Assert.IsTrue(res.Response);

            Assert.IsTrue(vm.Thermostats.Count > 0);
            Assert.AreEqual("Unit Name - Test", vm.Thermostats[0].UnitName);
        }
    }
}