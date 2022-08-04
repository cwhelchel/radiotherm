using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;
using RadioThermLib.Models;
using RadioThermLib.ViewModels;

namespace RadioThermWpf.Views
{
    /// <summary>
    /// Interaction logic for ThermostatDetailsWindow.xaml
    /// </summary>
    public partial class ThermostatDetailsWindow : Window
    {
        public ThermostatDetailsWindow(ThermostatViewModel thermostatViewModel)
        {
            InitializeComponent();

            // we dont inject via Ioc container here.
            this.DataContext = thermostatViewModel;

            this.Loaded += (s, e) => ViewModel.IsActive = true;
            this.Unloaded += (s, e) => ViewModel.IsActive = false;

            thermostatViewModel.PropertyChanged += (sender, args) =>
            {
                if (args.PropertyName == nameof(thermostatViewModel.State))
                    ModifyTheme((sender as ThermostatViewModel)?.State);
            };

            if (thermostatViewModel.State != null)
                ModifyTheme(thermostatViewModel.State);
        }

        public ThermostatViewModel ViewModel => (ThermostatViewModel)DataContext;

        private void ChangeResourceDictionary(ThermostatModeEnum newMode)
        {
            var mergedDicts = this.Resources.MergedDictionaries;

            Console.WriteLine(mergedDicts.Count);

            Uri uri;

            if (newMode == ThermostatModeEnum.Cool)
                uri = new Uri($"pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.Blue.xaml");
            else if (newMode == ThermostatModeEnum.Heat)
                uri = new Uri($"pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.Red.xaml");
            else
                uri = new Uri($"pack://application:,,,/MaterialDesignColors;component/Themes/Recommended/Primary/MaterialDesignColor.BlueGrey.xaml");

            mergedDicts.Clear();
            mergedDicts.Add(new ResourceDictionary() { Source = uri });
        }


        private void ModifyTheme(ThermostatState? s)
        {
            if (s == null)
                return;

            ChangeResourceDictionary(s.ThermostatMode);
        }
    }
}
