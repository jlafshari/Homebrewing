using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace HomebrewApp
{
    [ValueConversion(typeof(double), typeof(SolidColorBrush))]
    public class SrmColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new SolidColorBrush(ColorUtility.GetColorFromSrm((double) value));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
