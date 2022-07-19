﻿using CommunityToolkit.Mvvm.DependencyInjection;
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

namespace RadioThermWpf.Views
{
    /// <summary>
    /// Interaction logic for ThermostatWindow.xaml
    /// </summary>
    public partial class ThermostatWindow : Window
    {
        public ThermostatWindow()
        {
            InitializeComponent();

            this.DataContext = Ioc.Default.GetRequiredService<ThermostatViewModel>();

            this.Loaded += (s, e) => ViewModel.IsActive = true;
            this.Unloaded += (s, e) => ViewModel.IsActive = false;
        }

        public ThermostatViewModel ViewModel => (ThermostatViewModel)DataContext;
    }
}
