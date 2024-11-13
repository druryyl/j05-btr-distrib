using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace btr.distrib.Helpers
{
    public static class DecFormatter
    {
        public static string ToStr(decimal number)
        {
            var decNumber = GetDecimalPlaces(number);
            if (decNumber == 0)
                return $"{number:N0}";

            if (decNumber == 1)
                return $"{number:N1}";

            return $"{number:N2}";
        }
        private static bool HasDecimalPlaces(decimal number)
        {
            return number % 1 != 0;
        }
        private static int GetDecimalPlaces(decimal number)
        {
            if (!HasDecimalPlaces(number))
                return 0;

            string[] parts = number.ToString().Split('.');
            if (parts.Length > 1)
                return parts[1].Length; // Length of decimal part

            return 0; // No decimal places
        }

    }
}
