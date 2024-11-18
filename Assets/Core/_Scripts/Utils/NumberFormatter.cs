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

            string formattedString = (suffixIndex == 0)
                ? formattedNumber.ToString("F0") // No decimal precision for numbers below 1000
                : formattedNumber.ToString("F2").Replace('.', ','); // Two decimals with comma separator

            return formattedString + suffixes[suffixIndex];
        }
    }
}