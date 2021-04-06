using System;
using System.Windows.Data;

namespace HomebrewApp.Converters
{
    [ValueConversion(typeof(DateTime), typeof(string))]
    public class DateTimeDisplayConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((DateTime)value).ToString("d");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return null;
        }
    }
}
