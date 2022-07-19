using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using RadioThermLib.Services;
using RadioThermWpf.Services;
using RadioThermLib.ViewModels;
using RadioThermLib.ProvidedServices;

namespace RadioThermWpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var services = new ServiceCollection()
                //.AddSingleton<ISettingsService, SettingsService>()
                .AddSingleton<ISettingsService, JsonSettingService>()
                .AddSingleton<IViewService, ViewService>()
                .AddSingleton<IThermostatService, ThermostatService>() // Provided Service
                .AddTransient<ThermostatViewModel>()
                .BuildServiceProvider();

            Ioc.Default.ConfigureServices(services);
        }
    }
}
