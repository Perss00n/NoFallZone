using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private static void ShowError(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
    }
}
