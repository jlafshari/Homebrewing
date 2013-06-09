using System;
using System.Text.RegularExpressions;

namespace BeerRecipeCore
{
    public static class EnumConverter
    {
        public static object Parse(Type enumType, string value)
        {
            string valueNoSpaces = value.Replace(" ", "");
            return Enum.Parse(enumType, valueNoSpaces);
        }

        public static string SaveToString(this Enum value)
        {
            string valueAsString = value.ToString();
            valueAsString = Regex.Replace(valueAsString, "([a-z])([A-Z])", "$1 $2");
            return valueAsString;
        }
    }
}
