using System;
using System.Collections.Generic;
using System.Linq;
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
        }

        public ThermostatViewModel ViewModel => (ThermostatViewModel)DataContext;
    }
}
