using System;
using System.Globalization;
using System.Windows.Data;
using BeerRecipeCore.Fermentables;
using Utility;

namespace HomebrewApp.Converters
{
    [ValueConversion(typeof(FermentableType), typeof(string))]
    public class FermentableTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((FermentableType)value).SaveToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
