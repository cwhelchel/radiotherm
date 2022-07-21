using System;
using System.Globalization;
using System.Windows.Data;
using RadioThermLib.Models;

namespace RadioThermWpf.ValueConverters
{
    public class ThermostatModeToImageValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ThermostatModeEnum v = (ThermostatModeEnum)value;

            switch (v)
            {
                case ThermostatModeEnum.Off:
                    break;
                case ThermostatModeEnum.Heat:
                    return "..\\icons8-fire-48.png";
                case ThermostatModeEnum.Cool:
                    return "..\\icons8-snowflake-48.png";
                case ThermostatModeEnum.Auto:
                    break;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
