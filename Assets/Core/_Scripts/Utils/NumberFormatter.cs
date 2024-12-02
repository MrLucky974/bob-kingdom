using System;

namespace LuckiusDev.Utils
{
    public static class NumberFormatter
    {
        private static readonly string[] suffixes = {
            "", "k", "m", "b", "t",      // Thousands to Trillions
            "qa", "qt", "sx", "sp", "oc", // Quadrillions to Octillions
            "no", "dc", "ud", "dd", "td", // Nonillions to Tredecillions
            "qad", "qid", "sxd", "spd", "ocd", // Quadecillions to Octodecillions
            "ned", "vg", "uvg", "dvg", "tvg", // Novemdecillions to Trevigintillions
            "qavg", "qivg", "sxvg", "spvg", "ocvg", // Quadavigintillions to Octovigintillions
            "nevg", "tg", "utg", "dtg", "ttg" // Novemvigintillions to Tritrigintillions
        };

        public static string FormatNumberWithSuffix(float number)
        {
            int suffixIndex = 0;
            while (number >= 1000f && suffixIndex < suffixes.Length - 1)
            {
                number /= 1000f;
                suffixIndex++;
            }

            string format = suffixIndex < 1 ? "F0" : "F2";
            string formattedNumber = number.ToString(format) + suffixes[suffixIndex];
            return formattedNumber;
        }

        public static string FormatNumberWithSuffix(int number)
        {
            int suffixIndex = 0;
            double formattedNumber = number;

            while (formattedNumber >= 1000 && suffixIndex < suffixes.Length - 1)
            {
                formattedNumber /= 1000.0;
                suffixIndex++;
            }

            if (suffixIndex == 0)
            {
                return formattedNumber.ToString("F0"); // No decimal for numbers below 1000
            }
            else
            {
                // Use custom rounding and formatting
                string formatted = Math.Round(formattedNumber, 2).ToString("0.##");
                return formatted + suffixes[suffixIndex];
            }
        }

        public static string FormatNumberWithSuffix(ulong number)
        {
            int suffixIndex = 0;
            double formattedNumber = number;

            while (formattedNumber >= 1000 && suffixIndex < suffixes.Length - 1)
            {
                formattedNumber /= 1000.0;
                suffixIndex++;
            }

            if (suffixIndex == 0)
            {
                return formattedNumber.ToString("F0"); // No decimal for numbers below 1000
            }
            else
            {
                // Use custom rounding and formatting
                string formatted = Math.Round(formattedNumber, 2).ToString("0.##");
                return formatted + suffixes[suffixIndex];
            }
        }
    }
}