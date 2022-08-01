using RadioThermLib.Services;
using RadioThermLib.ViewModels;

namespace RadioThermLibTests.Mocks
{
    internal class MockViewService : IViewService
    {
        public string SavedTitle { get; private set; } = "";

        public string SavedMsg { get; private set; } = "";

        public void ShowDialog(string title, string message)
        {
            SavedMsg = message;
            SavedTitle = title;
        }

        public void ShowThermostatDetails(ThermostatViewModel viewModel)
        {
        }
    }
}