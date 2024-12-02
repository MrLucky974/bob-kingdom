using System;

namespace LuckiusDev.Utils
{
    public static class NumberFormatter
    {
        public static string FormatNumberWithSuffix(float number)
        {
            string[] suffixes = { "", "a", "b", "c", "d" }; // Add more suffixes as needed

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
            string[] suffixes = { "", "k", "m", "b", "t" }; // Add more suffixes as needed
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