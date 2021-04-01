using System;
using System.Globalization;
using System.Windows.Data;
using BeerRecipeCore.Hops;
using Utility;

namespace HomebrewApp
{
    [ValueConversion(typeof(HopsUse), typeof(string))]
    public class HopsUseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((HopsUse) value).SaveToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
