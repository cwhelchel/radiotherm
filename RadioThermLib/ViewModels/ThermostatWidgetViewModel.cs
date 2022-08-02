using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using RadioThermLib.Services;

namespace RadioThermLib.ViewModels
{
    public class ThermostatWidgetViewModel : ObservableRecipient
    {
        private readonly ISettingsService settingsService;
        private readonly IViewService viewService;
        private ObservableCollection<ThermostatViewModel> thermostatVms = null!;
        private bool isUpdating;
        private object respLock = new object();


        public ThermostatWidgetViewModel(ISettingsService settingsService, IViewService viewService)
        {
            this.settingsService = settingsService;
            this.viewService = viewService;
            Thermostats = new ObservableCollection<ThermostatViewModel>();

            // we dont care about the message sent in here
            StartUpdateAllCommand = new AsyncRelayCommand(async () => await HandleUpdateAllMsg(null));
        }

        #region Properties

        public ObservableCollection<ThermostatViewModel> Thermostats
        {
            get => thermostatVms;
            set => SetProperty(ref thermostatVms, value);
        }

        public bool IsUpdating
        {
            get => isUpdating;
            set => SetProperty(ref isUpdating, value);
        }

        public IAsyncRelayCommand StartUpdateAllCommand { get; }

        #endregion //Properties 

        protected override void OnActivated()
        {
            RegisterMessages();
        }

        private void RegisterMessages()
        {
            MessageHandler<ThermostatWidgetViewModel, UpdateRequestMessage> handler = async (r, m) =>
            {
                // run the update and respond true.
                await HandleUpdateAllMsg(m);
            };

            Messenger.Register(this, handler);
        }

        private async Task HandleUpdateAllMsg(UpdateRequestMessage? m)
        {
            try
            {
                await UpdateAllDevices();
            }
            catch (Exception ex)
            {
                string msg = RadioThermLib.strings.ErrUpdatingMsg;
                this.viewService.ShowDialog(RadioThermLib.strings.ErrUpdatingTitle, $"{msg}: {ex.Message}");
                this.IsUpdating = false;
            }

            if (m != null)
            {
                lock (respLock)
                {
                    if (!m.HasReceivedResponse)
                        m.Reply(true);
                }
            }
        }

        private async Task UpdateAllDevices()
        {
            this.IsUpdating = true;

            Thermostats.Clear();

            var discovered = settingsService.GetValue<List<string>>("DiscoveredAddresses");
            var manual = settingsService.GetValue<List<string>>("ManualAddresses");

            Debug.Assert(discovered != null, nameof(discovered) + " != null");
            Debug.Assert(manual != null, nameof(manual) + " != null");

            foreach (var dev in discovered)
            {
                var vm = Ioc.Default.GetService<ThermostatViewModel>();
                Thermostats.Add(vm!);
                await vm!.UpdateAsync(dev);
            }

            foreach (var dev in manual)
            {
                var vm = Ioc.Default.GetService<ThermostatViewModel>();
                Thermostats.Add(vm!);
                await vm!.UpdateAsync(dev);
            }

            this.IsUpdating = false;
        }
    }
}
