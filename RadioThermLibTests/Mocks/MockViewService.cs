using RadioThermLib.Services;
using RadioThermLib.ViewModels;

namespace RadioThermLibTests.Mocks
{
    internal class MockViewService : IViewService
    {
        public void ShowDialog(string title, string message)
        {
        }

        public void ShowThermostatDetails(ThermostatViewModel viewModel)
        {
        }
    }
}