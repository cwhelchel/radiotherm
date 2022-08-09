using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using RadioThermLib.Models;
using RadioThermLib.Services;

namespace RadioThermLib.ViewModels
{
    public class ProgramViewModel : ObservableObject
    {
        private readonly ThermostatProgram program;

        private ObservableCollection<DayProgram?> dayPrograms;

        public ProgramViewModel(ThermostatProgram program)
        {
            this.program = program;

            this.DayPrograms = new ObservableCollection<DayProgram?>();

            for (int i = 0; i < 7; i++)
            {
                DayPrograms.Add(this.program.GetDayProgram(i));
            }
        }

        public ObservableCollection<DayProgram?> DayPrograms
        {
            get => dayPrograms;
            set => SetProperty(ref dayPrograms, value);
        }
    }
}
