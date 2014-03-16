namespace Utility
{
    public static class StringUtility
    {
        /// <summary>
        /// Determines if the given string is null or empty.
        /// </summary>
        public static bool IsNullOrEmpty(this string str)
        {
            return str == null || str.Length == 0;
        }
    }
}
