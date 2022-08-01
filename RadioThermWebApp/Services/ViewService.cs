using RadioThermLib.Services;
using RadioThermLib.ViewModels;
using RadioThermWebApp.Pages;
using RadioThermWebApp.Shared;

namespace RadioThermWebApp.Services
{
    public class ViewService : IViewService
    {
        public EventHandler<ShowDialogsEventArgs> OnShowDialog;

        public void ShowDialog(string title, string message)
        {
            OnShowDialog?.Invoke(this, new ShowDialogsEventArgs { Title=title, Message=message});
        }

        public void ShowThermostatDetails(ThermostatViewModel viewModel)
        {
            throw new NotImplementedException();
        }
    }

    public class ShowDialogsEventArgs : EventArgs
    {
        public string? Title { get; set; }
        public string? Message { get; set; }
    }
}
