using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RadioThermLib.ViewModels;

namespace RadioThermLib.Services
{
    public interface IViewService
    {
        void ShowDialog(string title, string message);

        void ShowThermostatDetails(ThermostatViewModel viewModel);
    }
}
