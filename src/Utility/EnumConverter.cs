using System;
using System.Text.RegularExpressions;

namespace Utility
{
    public static class EnumConverter
    {
        /// <summary>
        /// Performs the same conversion as <see cref="Enum.Parse"/>, except that spaces are removed from the value before converting.
        /// </summary>
        public static T Parse<T>(string value)
        {
            string valueNoSpaces = value.Replace(" ", "");
            return (T) Enum.Parse(typeof(T), valueNoSpaces);
        }

        /// <summary>
        /// Converts the value to a string, adding a space between each word (where a word begins with a capital letter).
        /// </summary>
        public static string SaveToString(this Enum value)
        {
            string valueAsString = value.ToString();
            valueAsString = Regex.Replace(valueAsString, "([a-z])([A-Z])", "$1 $2");
            return valueAsString;
        }
    }
}
