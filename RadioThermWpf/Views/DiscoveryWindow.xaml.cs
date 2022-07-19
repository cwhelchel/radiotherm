using CommunityToolkit.Mvvm.DependencyInjection;
using RadioThermLib.ViewModels;
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

namespace RadioThermWpf
{
    /// <summary>
    /// Interaction logic for DiscoveryWindow.xaml
    /// </summary>
    public partial class DiscoveryWindow : Window
    {
        public DiscoveryWindow()
        {
            InitializeComponent();

            this.DataContext = Ioc.Default.GetRequiredService<DiscoveryViewModel>();
        }

        public DiscoveryViewModel ViewModel => (DiscoveryViewModel)DataContext;
    }
}
