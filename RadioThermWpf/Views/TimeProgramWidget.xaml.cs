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
using MaterialDesignThemes.Wpf;
using RadioThermLib.ViewModels;

namespace RadioThermWpf.Views
{
    /// <summary>
    /// Interaction logic for TimeProgramWidget.xaml
    /// </summary>
    public partial class TimeProgramWidget : UserControl
    {
        public TimeProgramWidget()
        {
            InitializeComponent();
        }

        public TimeProgramViewModel ViewModel => (TimeProgramViewModel)DataContext;

        public void ClockDialogOpenedEventHandler(object sender, DialogOpenedEventArgs eventArgs)
            => Clock.Time = DateTime.Today.Add(ViewModel.StartTime);

        public void ClockDialogClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            //if (Equals(eventArgs.Parameter, "1"))
            //    ((TimeProgramViewModel)DataContext).Time = Clock.Time;
        }
    }
}
