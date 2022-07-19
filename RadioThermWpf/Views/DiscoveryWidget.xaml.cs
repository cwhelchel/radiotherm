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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RadioThermWpf.Views
{
    /// <summary>
    /// Interaction logic for DiscoveryWidget.xaml
    /// </summary>
    public partial class DiscoveryWidget : UserControl
    {
        public DiscoveryWidget()
        {
            InitializeComponent();
            this.DataContext = Ioc.Default.GetRequiredService<DiscoveryWidgetViewModel>();

            this.Loaded += (s, e) => ViewModel.IsActive = true;
            this.Unloaded += (s, e) => ViewModel.IsActive = false;
        }

        public DiscoveryWidgetViewModel ViewModel => (DiscoveryWidgetViewModel)DataContext;
    }
}
