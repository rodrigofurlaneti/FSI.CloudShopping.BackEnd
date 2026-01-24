namespace FSI.CloudShopping.Domain.ValidationAssistant
{
    internal static class DocumentValidator
    {
        public static bool Validate(string doc, int baseLength, int[] m1, int[] m2)
        {
            string seed = doc.Substring(0, baseLength);
            string d1 = CalculateDigit(seed, m1);
            string d2 = CalculateDigit(seed + d1, m2);
            return doc.EndsWith(d1 + d2);
        }

        private static string CalculateDigit(string baseNum, int[] weights)
        {
            int sum = 0;
            for (int i = 0; i < weights.Length; i++)
                sum += (baseNum[i] - '0') * weights[i];

            int remainder = sum % 11;
            int digit = remainder < 2 ? 0 : 11 - remainder;
            return digit.ToString();
        }
    }
}
