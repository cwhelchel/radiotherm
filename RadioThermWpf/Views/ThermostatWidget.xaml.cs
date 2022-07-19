using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using RadioThermLib;
using RadioThermLib.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RadioThermWpf.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ThermostatWidget : UserControl
    {
        public ThermostatWidget()
        {
            InitializeComponent();

            //this.DataContext = Ioc.Default.GetRequiredService<ThermostatViewModel>();
        }

        public ThermostatViewModel ViewModel => (ThermostatViewModel)DataContext;
    }
}
