using System;
using System.Collections.Generic;
using System.Text;

namespace DataStructures.Extensions
{
    public static class StringExtensions
    {
        public static bool HasValue(this string? value)
        {
            return !string.IsNullOrEmpty(value);
        }

        public static bool EquelsIgnoreCase(this string value1, string value2)
        {
            return value1.Equals(value2,StringComparison.OrdinalIgnoreCase);
        }
    }
}
