﻿using System;
using System.Globalization;
using System.Windows.Data;
using BeerRecipeCore;
using Utility;

namespace HomebrewApp
{
    [ValueConversion(typeof(FermentableType), typeof(string))]
    public class FermentableTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((FermentableType) value).SaveToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
