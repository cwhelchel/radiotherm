using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using RadioThermLib.Models;

namespace RadioThermLib.ViewModels
{
    public class ProgramViewModel : ObservableObject
    {
        private ObservableCollection<DayProgramViewModel?> dayPrograms;

        public ProgramViewModel(ThermostatProgram program)
        {
            this.dayPrograms = new ObservableCollection<DayProgramViewModel?>();

            for (int i = 0; i < 7; i++)
            {
                DayPrograms.Add(new DayProgramViewModel(program.GetDayProgram(i)!));
            }
        }

        public ObservableCollection<DayProgramViewModel?> DayPrograms
        {
            get => dayPrograms;
            set => SetProperty(ref dayPrograms, value);
        }
    }

    public class DayProgramViewModel : ObservableObject
    {
        private string day;
        private ObservableCollection<TimeProgramViewModel> data;

        public DayProgramViewModel(DayProgram dayProgram)
        {
            this.day = dayProgram.Day.ToString();
            this.data = new ObservableCollection<TimeProgramViewModel>(
                dayProgram.ProgramData.ConvertAll(input => new TimeProgramViewModel(input)));
        }

        public string DayOfWeek
        {
            get => day;
            set => SetProperty(ref day, value);
        }

        public ObservableCollection<TimeProgramViewModel> Data
        {
            get => data;
            set => SetProperty(ref data, value);
        }
    }

    public class TimeProgramViewModel : ObservableObject
    {
        private float programmedTemp;
        private TimeSpan startTime;

        public TimeProgramViewModel(TimeProgram data)
        {
            this.programmedTemp = data.ProgrammedTemp;
            this.startTime = data.Time;
        }

        public float ProgrammedTemp
        {
            get => programmedTemp;
            set => SetProperty(ref programmedTemp, value);
        }

        public TimeSpan StartTime
        {
            get => startTime;
            set => SetProperty(ref startTime, value);
        }
    }
}
