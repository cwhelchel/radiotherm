using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using RadioThermLib.Models;
using RadioThermLib.Services;

namespace RadioThermLib.ViewModels
{
    public class ProgramViewModel : ObservableObject
    {
        private ThermostatProgram program;
        private ObservableCollection<DayProgramViewModel?> dayPrograms;

        private static Dictionary<string, int> map = new()
        {
            { "Monday",    0 },
            { "Tuesday",   1 },
            { "Wednesday", 2 },
            { "Thursday",  3 },
            { "Friday",    4 },
            { "Saturday",  5 },
            { "Sunday",    6 },
        };

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

        public ThermostatProgram GetProgram()
        {
            var tp = new ThermostatProgram(
                DayPrograms[0].GetDayProgram(),
                DayPrograms[1].GetDayProgram(),
                DayPrograms[2].GetDayProgram(),
                DayPrograms[3].GetDayProgram(),
                DayPrograms[4].GetDayProgram(),
                DayPrograms[5].GetDayProgram(),
                DayPrograms[6].GetDayProgram()
            );

            return tp;
        }
    }

    public class DayProgramViewModel : ObservableObject
    {
        private string day;
        private readonly int dayIdx;
        private ObservableCollection<TimeProgramViewModel> data;

        public DayProgramViewModel(DayProgram dayProgram)
        {
            this.day = dayProgram.Day.ToString();
            this.dayIdx = dayProgram.DayIndex;
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

        public List<int> GetDayProgram()
        {
            var data = new List<int>();
            foreach (var timeProgramViewModel in Data)
            {
                var tp = timeProgramViewModel.GetTimeProgram();
                data.Add(tp.TimeInt);
                data.Add((int)tp.ProgrammedTemp);
            }

            return data;
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

        public TimeProgram GetTimeProgram()
        {
            return new TimeProgram((int)startTime.TotalMinutes, (int)programmedTemp);
        }
    }
}
