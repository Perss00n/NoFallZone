using NoFallZone.Data;
using NoFallZone.Models.Enums;
using NoFallZone.Utilities.Helpers;

namespace NoFallZone.Utilities.Validators
{
    public static class CustomerValidator
    {
        public const int MinNameLength = 4;
        public const int MaxNameLength = 50;

        public const int MinUsernameLength = 3;
        public const int MaxUsernameLength = 50;

        public const int MinPasswordLength = 8;
        public const int MaxPasswordLength = 100;

        public const int MaxEmailLength = 70;

        public const int MaxPhoneLength = 20;

        public const int MinAddressLength = 5;
        public const int MaxAddressLength = 100;

        public const int MinPostalCodeLength = 5;
        public const int MaxPostalCodeLength = 6; 

        public const int MinCityLength = 2;
        public const int MaxCityLength = 50;

        public const int MinCountryLength = 2;
        public const int MaxCountryLength = 55;

        public const int MinAge = 1;
        public const int MaxAge = 100;

        public static string PromptName() =>
            InputHelper.PromptRequiredLimitedString("Enter your full name", MinNameLength, MaxNameLength,
                $"Name must be between {MinNameLength} and {MaxNameLength} characters.");

        public static string PromptUsername(NoFallZoneContext db) =>
            InputHelper.PromptUsername("Enter a username", MinUsernameLength, MaxUsernameLength,
                $"Username must be between {MinUsernameLength} and {MaxUsernameLength} characters.", db);

        public static Role PromptRole() =>
            InputHelper.PromptRole("Enter user role", "Role must be 'user' or 'admin'.");

        public static string PromptPassword() =>
            InputHelper.PromptPassword("Enter a password", MinPasswordLength, MaxPasswordLength,
                $"Password must be between {MinPasswordLength} and {MaxPasswordLength} characters.");

        public static string PromptEmail(NoFallZoneContext db) =>
            InputHelper.PromptEmail("Enter your email", MaxEmailLength,
                $"Email must be valid and can't exceed {MaxEmailLength} characters.", db);

        public static string PromptPhone() =>
            InputHelper.PromptPhone("Enter your phone number", MaxPhoneLength,
                $"Phone number must be valid and can't exceed {MaxPhoneLength} characters.");

        public static string PromptAddress() =>
            InputHelper.PromptRequiredLimitedString("Enter your address", MinAddressLength, MaxAddressLength,
                $"Address can't be empty and must be between {MinAddressLength} and {MaxAddressLength} characters.");

        public static string PromptPostalCode() =>
            InputHelper.PromptPostalCode("Enter your postal code", MinPostalCodeLength, MaxPostalCodeLength,
                $"Postal code must be {MinPostalCodeLength} or {MaxPostalCodeLength} digits and follow format 12345 or 123 45.");

        public static string PromptCity() =>
            InputHelper.PromptRequiredLimitedString("Enter your city", MinCityLength, MaxCityLength,
                $"City can't be empty and must be between {MinCityLength} and {MaxCityLength} characters.");

        public static string PromptCountry() =>
            InputHelper.PromptRequiredLimitedString("Enter your country", MinCountryLength, MaxCountryLength,
                $"Country can't be empty and must be between {MinCountryLength} and {MaxCountryLength} characters.");

        public static int PromptAge() =>
            InputHelper.PromptInt("Enter your age", MinAge, MaxAge,
                $"Enter a valid age between {MinAge} and {MaxAge}.");

        public static bool PromptConfirmation() =>
            InputHelper.PromptYesNo("Confirm", "Please enter only 'Y' for Yes and 'N' for No.");

        public static string? PromptOptionalName(string currentName) =>
            InputHelper.PromptOptionalLimitedString($"Name [{currentName}]", MinNameLength, MaxNameLength,
                $"Name must be between {MinNameLength} and {MaxNameLength} characters.");

        public static string? PromptOptionalEmail(NoFallZoneContext db, string currentEmail) =>
            InputHelper.PromptOptionalEmail($"Email [{currentEmail}]", MaxEmailLength,
                $"Email must be valid and not exceed {MaxEmailLength} characters.", db, currentEmail);

        public static string? PromptOptionalPhone(string current) =>
            InputHelper.PromptOptionalPhone($"Phone [{current}]", MaxPhoneLength,
                $"The phone number can't exceed {MaxPhoneLength} characters.");

        public static string? PromptOptionalAddress(string currentAddress) =>
            InputHelper.PromptOptionalLimitedString($"Address [{currentAddress}]", MinAddressLength, MaxAddressLength,
                $"Address must be between {MinAddressLength} and {MaxAddressLength} characters.");

        public static string? PromptOptionalPostalCode(string current) =>
            InputHelper.PromptOptionalPostalCode($"Postal code [{current}]", MinPostalCodeLength, MaxPostalCodeLength,
                $"Postal code must be {MinPostalCodeLength} or {MaxPostalCodeLength} digits and follow format 12345 or 123 45.");

        public static string? PromptOptionalCity(string currentCity) =>
            InputHelper.PromptOptionalLimitedString($"City [{currentCity}]", MinCityLength, MaxCityLength,
                $"City must be between {MinCityLength} and {MaxCityLength} characters.");

        public static string? PromptOptionalCountry(string currentCountry) =>
            InputHelper.PromptOptionalLimitedString($"Country [{currentCountry}]", MinCountryLength, MaxCountryLength,
                $"Country must be between {MinCountryLength} and {MaxCountryLength} characters.");

        public static int? PromptOptionalAge(int current) =>
            InputHelper.PromptOptionalInt($"Age [{current}]", MinAge, MaxAge,
                $"Enter a valid age between {MinAge} and {MaxAge}.");

        public static string? PromptOptionalPassword(string current) =>
            InputHelper.PromptOptionalPassword($"Password [hidden]", MinPasswordLength, MaxPasswordLength,
                $"Password must be between {MinPasswordLength} and {MaxPasswordLength} characters.");

        public static Role? PromptOptionalRole(Role current) =>
            InputHelper.PromptOptionalRole($"Role [{current}]", "Role must be 'user' or 'admin'.");

        public static string? PromptOptionalUsername(NoFallZoneContext db, string currentUsername) =>
            InputHelper.PromptOptionalUsername($"Username [{currentUsername}]", MinUsernameLength, MaxUsernameLength,
                $"Username must be between {MinUsernameLength} and {MaxUsernameLength} characters.", db, currentUsername);


    }
}
