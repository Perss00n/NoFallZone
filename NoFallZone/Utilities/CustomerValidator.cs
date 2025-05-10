using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoFallZone.Utilities
{
    public static class CustomerValidator
    {
        public const int MaxNameLength = 50;
        public const int MaxEmailLength = 70;
        public const int MaxPhoneLength = 20;
        public const int MaxAddressLength = 100;
        public const int MaxPostalCodeLength = 6;
        public const int MaxCityLength = 50;
        public const int MaxCountryLength = 55;
        public const int MinAge = 1;
        public const int MaxAge = 100;

        public static string PromptName() =>
            InputHelper.PromptRequiredLimitedString("Enter your full name", MaxNameLength,
                $"Name can't be empty and can't exceed {MaxNameLength} characters.");

        public static string PromptEmail() =>
            InputHelper.PromptEmail("Enter your email", MaxEmailLength,
                $"Email must be valid and can't exceed {MaxEmailLength} characters.");

        public static string PromptPhone() =>
            InputHelper.PromptPhone("Enter your phone number", MaxPhoneLength,
                $"Phone number must be valid and can't exceed {MaxPhoneLength} characters.");

        public static string PromptAddress() =>
            InputHelper.PromptRequiredLimitedString("Enter your address", MaxAddressLength,
                $"Address can't be empty and can't exceed {MaxAddressLength} characters.");

        public static string PromptPostalCode() =>
           InputHelper.PromptPostalCode("Enter your postal code", MaxPostalCodeLength,
                $"Postal code can't be empty and can't exceed {MaxPostalCodeLength} characters.");

        public static string PromptCity() =>
            InputHelper.PromptRequiredLimitedString("Enter your city", MaxCityLength,
                $"City can't be empty and can't exceed {MaxCityLength} characters.");

        public static string PromptCountry() =>
            InputHelper.PromptRequiredLimitedString("Enter your country", MaxCountryLength,
                $"Country can't be empty and can't exceed {MaxCountryLength} characters.");

        public static int PromptAge() =>
            InputHelper.PromptInt("Enter your age", MinAge, MaxAge,
                $"Enter a valid age between {MinAge} and {MaxAge}.");
    }
}
