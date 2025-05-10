using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NoFallZone.Utilities
{
    public static class InputHelper
    {
        public static string Prompt(string label, Func<string, bool> isValid, string errorMsg)
        {
            string input;
            do
            {
                Console.Write($"{label}: ");
                input = Console.ReadLine()!;
                if (!isValid(input))
                {
                    ShowError(errorMsg);
                }
            } while (!isValid(input));
            return input;
        }

        public static int PromptInt(string label, int min, int max, string errorMsg)
        {
            int value;
            string input;
            bool isValid;
            do
            {
                Console.Write($"{label} ({min} - {max}): ");
                input = Console.ReadLine()!;
                isValid = int.TryParse(input, out value) && value >= min && value <= max;

                if (!isValid)
                {
                    ShowError(errorMsg);
                }

            } while (!isValid);
            return value;
        }

        public static decimal PromptDecimal(string label, decimal min, decimal max, string errorMsg)
        {
            decimal value;
            string input;
            bool isValid;
            do
            {
                Console.Write($"{label} ({min} - {max}): ");
                input = Console.ReadLine()!;
                isValid = decimal.TryParse(input, out value) && value >= min && value <= max;

                if (!isValid)
                {
                    ShowError(errorMsg);
                }

            } while (!isValid);
            return value;
        }

        public static int? PromptOptionalInt(string label, int min, int max, string errorMsg)
        {
            string input;
            int value;
            do
            {
                Console.Write($"{label} (Min '{min}' and Max '{max}', Press enter too keep the current value as it is): ");
                input = Console.ReadLine()!;
                if (string.IsNullOrWhiteSpace(input))
                    return null;

                if (int.TryParse(input, out value) && value >= min && value <= max)
                    return value;

                ShowError(errorMsg);

            } while (true);
        }

        public static decimal? PromptOptionalDecimal(string label, decimal min, decimal max, string errorMsg)
        {
            string input;
            decimal value;
            do
            {
                Console.Write($"{label} (Min '{min}' and Max '{max}', Press enter too keep the current value as it is): ");
                input = Console.ReadLine()!;
                if (string.IsNullOrWhiteSpace(input))
                    return null;

                if (decimal.TryParse(input, out value) && value >= min && value <= max)
                    return value;

                ShowError(errorMsg);

            } while (true);
        }

        public static string PromptOptionalLimitedString(string label, int maxLength, string errorMsg)
        {
            string input;
            do
            {
                Console.Write($"{label} (Max {maxLength} chars, Press enter too keep the current value as it is): ");
                input = Console.ReadLine()!;

                if (string.IsNullOrWhiteSpace(input))
                    return "";

                if (input.Length <= maxLength)
                    return input;

                ShowError(errorMsg);

            } while (true);
        }


        public static string PromptRequiredLimitedString(string label, int maxLength, string errorMsg)
        {
            string input;
            do
            {
                Console.Write($"{label} (Max {maxLength} chars): ");
                input = Console.ReadLine()!;

                if (!string.IsNullOrWhiteSpace(input) && input.Length <= maxLength)
                    return input;

                ShowError(errorMsg);

            } while (true);
        }

        public static bool PromptYesNo(string label, string errorMsg)
        {
            string input;
            do
            {
                Console.Write($"{label} (Y/N): ");
                input = Console.ReadLine()!.Trim().ToUpper();

                if (input == "Y") return true;
                if (input == "N") return false;

                ShowError(errorMsg);

            } while (true);
        }

        public static string PromptPhone(string label, int maxLength, string errorMsg)
        {
            string input;

            // Regex för att matcha telefonnummer med minst 7 tecken med format som +46, (070), 070-123 45 67 etc.
            var phoneRegex = new Regex(@"^\+?[0-9\s\-()]{7,}$");

            do
            {
                Console.Write($"{label} (max {maxLength} chars): ");
                input = Console.ReadLine()!.Trim();

                if (!string.IsNullOrWhiteSpace(input) &&
                    input.Length <= maxLength &&
                    phoneRegex.IsMatch(input))
                {
                    return input;
                }

                ShowError(errorMsg);
            } while (true);
        }


        public static string PromptEmail(string label, int maxLength, string errorMsg)
        {
            string input;

            // Regex för att matcha enkla e-postadresser som marcus@nofallzone.com, marcus.lehm@nofallzone.com etc.
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            do
            {
                Console.Write($"{label} (max {maxLength} chars): ");
                input = Console.ReadLine()!.Trim();

                if (!string.IsNullOrWhiteSpace(input) &&
                    input.Length <= maxLength &&
                    emailRegex.IsMatch(input))
                {
                    return input;
                }

                ShowError(errorMsg);
            } while (true);
        }

        public static string PromptPostalCode(string label, int maxLength, string errorMsg)
        {
            string input;

            // Tillåt svensk form: 3 siffror + valfritt mellanslag + 2 siffror (t.ex. "12345" eller "123 45")
            var postalRegex = new Regex(@"^\d{3}\s?\d{2}$");

            do
            {
                Console.Write($"{label} (Ex 45141 or 451 41): ");
                input = Console.ReadLine()!.Trim();

                if (!string.IsNullOrWhiteSpace(input)
                    && input.Length <= maxLength
                    && postalRegex.IsMatch(input))
                {
                    return input;
                }

                ShowError(errorMsg);

            } while (true);
        }



        private static void ShowError(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ResetColor();
        }

    }
}
