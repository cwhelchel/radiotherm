using System;
using System.Globalization;
using System.Windows.Data;
using RadioThermLib.Models;

namespace RadioThermWpf
{
    public class ThermostatModeToTextValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ThermostatModeEnum v = (ThermostatModeEnum)value;

            switch (v)
            {
                case ThermostatModeEnum.Off:
                    return "Off";
                case ThermostatModeEnum.Heat:
                    return "Heat";
                case ThermostatModeEnum.Cool:
                    return "Cool";
                case ThermostatModeEnum.Auto:
                    return "Auto";
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
