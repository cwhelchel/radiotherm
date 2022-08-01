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
using RadioThermLibTests.Mocks;

namespace RadioThermLibTests
{
    [TestClass()]
    public class TestInitializer
    {
        [AssemblyInitialize()]
        public static void SetupIoc(TestContext tc)
        {
            Ioc.Default.ConfigureServices(new ServiceCollection()
                 .AddSingleton<ISettingsService, MockSettingsService>()
                 .AddSingleton<IThermostatService, MockThermostatService>()
                 .AddSingleton<IViewService, MockViewService>()
                 .AddTransient<ThermostatWidgetViewModel>()
                 .AddTransient<DiscoveryWidgetViewModel>()
                 .AddTransient<ThermostatViewModel>()
                 .BuildServiceProvider());
        }
    }
}
