using System.Text.RegularExpressions;
using NoFallZone.Data;
using NoFallZone.Models.Enums;

namespace NoFallZone.Utilities.Helpers
{
    public static class InputHelper
    {
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
                    OutputHelper.ShowError(errorMsg);
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
                    OutputHelper.ShowError(errorMsg);
                }

            } while (!isValid);
            return value;
        }


        public static string PromptRequiredLimitedString(string label, int minLength, int maxLength, string errorMsg)
        {
            string input;
            do
            {
                Console.Write($"{label} ({minLength}-{maxLength} chars): ");
                input = Console.ReadLine()!;

                if (!string.IsNullOrWhiteSpace(input) &&
                    input.Length >= minLength &&
                    input.Length <= maxLength)
                {
                    return input;
                }

                OutputHelper.ShowError(errorMsg);

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

                OutputHelper.ShowError(errorMsg);

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

                OutputHelper.ShowError(errorMsg);
            } while (true);
        }


        public static string PromptEmail(string label, int maxLength, string errorMsg, NoFallZoneContext db)
        {
            string input;
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            do
            {
                Console.Write($"{label} (max {maxLength} chars): ");
                input = Console.ReadLine()!.Trim();

                bool isValidEmail = !string.IsNullOrWhiteSpace(input) &&
                                    input.Length <= maxLength &&
                                    emailRegex.IsMatch(input);

                bool isUnique = !db.Customers.Any(c => c.Email == input);

                if (isValidEmail && isUnique)
                    return input;

                if (!isValidEmail)
                    OutputHelper.ShowError(errorMsg);
                else if (!isUnique)
                    OutputHelper.ShowError("That email is already registered! Try a different one...");
            }
            while (true);
        }

        public static string PromptPostalCode(string label, int minLength, int maxLength, string errorMsg)
        {
            string input;

            // Tillåt svensk form: 3 siffror + valfritt mellanslag + 2 siffror (t.ex. "12345" eller "123 45")
            var postalRegex = new Regex(@"^\d{3}\s?\d{2}$");

            do
            {
                Console.Write($"{label} (Ex 45141 or 451 41): ");
                input = Console.ReadLine()!.Trim();

                if (!string.IsNullOrWhiteSpace(input)
                    && input.Length >= minLength
                    && input.Length <= maxLength
                    && postalRegex.IsMatch(input))
                {
                    return input;
                }

                OutputHelper.ShowError(errorMsg);

            } while (true);
        }


        public static string PromptUsername(string label, int minLength, int maxLength, string errorMsg, NoFallZoneContext db)
        {
            string input;

            do
            {
                Console.Write($"{label} ({minLength}-{maxLength} chars): ");
                input = Console.ReadLine()!.Trim();

                bool isValid = !string.IsNullOrWhiteSpace(input) &&
                               input.Length >= minLength &&
                               input.Length <= maxLength;

                bool isUnique = !db.Customers.Any(c => c.Username == input);

                if (isValid && isUnique)
                    return input;

                if (!isValid)
                    OutputHelper.ShowError(errorMsg);
                else if (!isUnique)
                    OutputHelper.ShowError("That username is already taken! Try a different one...");
            }
            while (true);
        }

        public static string PromptPassword(string label, int minLength, int maxLength, string errorMsg)
        {
            string input;

            do
            {
                Console.Write($"{label} ({minLength}-{maxLength} chars): ");
                input = Console.ReadLine()!.Trim();

                if (!string.IsNullOrWhiteSpace(input) &&
                    input.Length >= minLength &&
                    input.Length <= maxLength)
                {
                    return input;
                }

                OutputHelper.ShowError(errorMsg);
            }
            while (true);
        }

        public static Role PromptRole(string label, string errorMsg)
        {
            do
            {
                Console.Write($"{label} (User/Admin): ");
                string input = Console.ReadLine()!.Trim();

                if (Enum.TryParse<Role>(input, true, out Role role))
                    return role;

                OutputHelper.ShowError(errorMsg);
            }
            while (true);
        }

        public static int? PromptOptionalInt(string label, int min, int max, string errorMsg)
        {
            string input;
            int value;
            do
            {
                Console.Write($"{label} (Min '{min}' and Max '{max}, enter to keep current): ");
                input = Console.ReadLine()!;
                if (string.IsNullOrWhiteSpace(input))
                    return null;

                if (int.TryParse(input, out value) && value >= min && value <= max)
                    return value;

                OutputHelper.ShowError(errorMsg);

            } while (true);
        }

        public static decimal? PromptOptionalDecimal(string label, decimal min, decimal max, string errorMsg)
        {
            string input;
            decimal value;
            do
            {
                Console.Write($"{label} (Min '{min}' and Max '{max}, enter to keep current): ");
                input = Console.ReadLine()!;
                if (string.IsNullOrWhiteSpace(input))
                    return null;

                if (decimal.TryParse(input, out value) && value >= min && value <= max)
                    return value;

                OutputHelper.ShowError(errorMsg);

            } while (true);
        }

        public static string? PromptOptionalLimitedString(string label, int minLength, int maxLength, string errorMsg)
        {
            string input;

            do
            {
                Console.Write($"{label} ({minLength}-{maxLength} chars, enter to keep current): ");
                input = Console.ReadLine()!.Trim();

                if (string.IsNullOrWhiteSpace(input))
                    return null;

                if (input.Length >= minLength && input.Length <= maxLength)
                    return input;

                OutputHelper.ShowError(errorMsg);
            }
            while (true);
        }


        public static string? PromptOptionalPhone(string label, int maxLength, string errorMsg)
        {
            string input;
            var phoneRegex = new Regex(@"^\+?[0-9\s\-()]{7,}$");

            do
            {
                Console.Write($"{label} (Max {maxLength} chars, enter to keep current): ");
                input = Console.ReadLine()!.Trim();

                if (string.IsNullOrWhiteSpace(input))
                    return null;

                if (input.Length <= maxLength && phoneRegex.IsMatch(input))
                    return input;

                OutputHelper.ShowError(errorMsg);
            } while (true);
        }


        public static string? PromptOptionalEmail(string label, int maxLength, string errorMsg, NoFallZoneContext db, string currentEmail)
        {
            string input;
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            do
            {
                Console.Write($"{label} (Max {maxLength}, enter to keep current): ");
                input = Console.ReadLine()!.Trim();

                if (string.IsNullOrWhiteSpace(input))
                    return null;

                bool isValid = input.Length <= maxLength && emailRegex.IsMatch(input);
                bool isUnique = input == currentEmail || !db.Customers.Any(c => c.Email == input);

                if (isValid && isUnique)
                    return input;

                if (!isValid)
                    OutputHelper.ShowError(errorMsg);
                else if (!isUnique)
                    OutputHelper.ShowError("That email is already registered. Try another.");
            }
            while (true);
        }


        public static string? PromptOptionalPostalCode(string label, int minLength, int maxLength, string errorMsg)
        {
            string input;

            var postalRegex = new Regex(@"^\d{3}\s?\d{2}$");

            do
            {
                Console.Write($"{label} (Ex 45141 or 451 41, enter to keep current): ");
                input = Console.ReadLine()!.Trim();

                if (string.IsNullOrWhiteSpace(input))
                    return null;

                if (input.Length >= minLength && input.Length <= maxLength && postalRegex.IsMatch(input))
                    return input;

                OutputHelper.ShowError(errorMsg);
            }
            while (true);
        }

        public static string? PromptOptionalPassword(string label, int minLength, int maxLength, string errorMsg)
        {
            string input;

            do
            {
                Console.Write($"{label} ({minLength}-{maxLength} chars, enter to keep current): ");
                input = Console.ReadLine()!.Trim();

                if (string.IsNullOrWhiteSpace(input))
                    return null;

                if (input.Length >= minLength && input.Length <= maxLength)
                    return input;

                OutputHelper.ShowError(errorMsg);
            }
            while (true);
        }


        public static Role? PromptOptionalRole(string label, string errorMsg)
        {
            Console.Write($"{label} (user/admin, enter to keep current): ");
            string input = Console.ReadLine()!.Trim();

            if (string.IsNullOrWhiteSpace(input))
                return null;

            if (Enum.TryParse<Role>(input, true, out Role role))
                return role;

            OutputHelper.ShowError(errorMsg);
            return PromptOptionalRole(label, errorMsg);
        }


        public static string? PromptOptionalUsername(string label, int minLength, int maxLength, string errorMsg, NoFallZoneContext db, string currentUsername)
        {
            string input;

            do
            {
                Console.Write($"{label} ({minLength}-{maxLength} chars, enter to keep current): ");
                input = Console.ReadLine()!.Trim();

                if (string.IsNullOrWhiteSpace(input))
                    return null;

                bool isValid = input.Length >= minLength && input.Length <= maxLength;
                bool isUnique = input == currentUsername || !db.Customers.Any(c => c.Username == input);

                if (isValid && isUnique)
                    return input;

                if (!isValid)
                    OutputHelper.ShowError(errorMsg);
                else if (!isUnique)
                    OutputHelper.ShowError("That username is already taken. Try another.");
            }
            while (true);
        }

    }
}
