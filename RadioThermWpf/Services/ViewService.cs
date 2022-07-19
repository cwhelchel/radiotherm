using CommunityToolkit.Mvvm.ComponentModel;
using RadioThermLib.Services;
using RadioThermLib.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RadioThermWpf.Services
{
    public class ViewService : IViewService
    {
        public void ShowDialog(string title, string message)
        {
            MessageBox.Show(message, title);
        }
    }
}
