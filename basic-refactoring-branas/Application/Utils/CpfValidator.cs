using System.Text.RegularExpressions;

namespace basic_refactoring_branas.Application.Utils
{
    public static class CpfValidator
    {
        private const int CPF_LENGTH = 11;
        private const int FACTOR_FIRST_DIGIT = 10;
        private const int FACTOR_SECOND_DIGIT = 11;

        public static bool ValidateCpf(string rawCpf)
        {
            if (string.IsNullOrEmpty(rawCpf))
            {
                return false;
            }

            string cpf = RemoveNonDigits(rawCpf);

            if (cpf.Length != CPF_LENGTH)
            {
                return false;
            }

            if (AllDigitsTheSame(cpf))
            {
                return false;
            }

            int digit1 = CalculateDigit(cpf, FACTOR_FIRST_DIGIT);
            int digit2 = CalculateDigit(cpf, FACTOR_SECOND_DIGIT);

            string actualDigit = ExtractActualDigit(cpf);
            return actualDigit == $"{digit1}{digit2}";
        }

        private static string RemoveNonDigits(string cpf)
        {
            return Regex.Replace(cpf, @"\D", "");
        }

        private static bool AllDigitsTheSame(string cpf)
        {
            char firstDigit = cpf[0];
            return cpf.All(digit => digit == firstDigit);
        }

        private static int CalculateDigit(string cpf, int factor)
        {
            int total = 0;
            foreach (char digit in cpf.ToCharArray())
            {
                if (factor > 1)
                {
                    int digitint = int.Parse(digit.ToString());
                    total += digitint * factor--;
                }
            }

            int remainder = total % 11;
            return remainder < 2 ? 0 : 11 - remainder;
        }

        private static string ExtractActualDigit(string cpf)
        {
            return cpf.Substring(9);
        }
    }

}
