using NoFallZone.Utilities.Helpers;

namespace NoFallZone.Utilities.Validators
{
    public static class PaymentOptionValidator
    {
        public const int MinNameLength = 3;
        public const int MaxNameLength = 40;
        public const decimal MinFee = 0;
        public const decimal MaxFee = 500;

        public static string PromptName() =>
            InputHelper.PromptRequiredLimitedString(
                "Enter payment option name",
                MinNameLength,
                MaxNameLength,
                $"Name must be between {MinNameLength} and {MaxNameLength} characters."
            );

        public static decimal PromptFee() =>
            InputHelper.PromptDecimal(
                "Enter fee for the payment option",
                MinFee,
                MaxFee,
                $"Fee must be between {MinFee:C} and {MaxFee:C}."
            );

        public static string? PromptOptionalName(string currentName) =>
            InputHelper.PromptOptionalLimitedString(
                $"Name [{currentName}]",
                MinNameLength,
                MaxNameLength,
                $"Name must be between {MinNameLength} and {MaxNameLength} characters."
            );

        public static decimal? PromptOptionalFee(decimal currentFee) =>
            InputHelper.PromptOptionalDecimal(
                $"Fee [{currentFee}]",
                MinFee,
                MaxFee,
                $"Fee must be between {MinFee:C} and {MaxFee:C}."
            );

        public static bool PromptConfirmation() =>
            InputHelper.PromptYesNo("Confirm", "Please enter only 'Y' for Yes and 'N' for No.");
    }
}