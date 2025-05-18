using NoFallZone.Utilities.Helpers;

namespace NoFallZone.Utilities.Validators;
public static class SupplierValidator
{
    public const int MinNameLength = 3;
    public const int MaxNameLength = 40;

    public static string PromptName() =>
        InputHelper.PromptRequiredLimitedString("Enter supplier name", MinNameLength, MaxNameLength,
            $"The supplier name can't be empty and must be between {MinNameLength} and {MaxNameLength} chars!");

    public static string? PromptOptionalName(string currentValue) =>
        InputHelper.PromptOptionalLimitedString($"Name [{currentValue}]", MinNameLength, MaxNameLength,
            $"The supplier Name must be between {MinNameLength} and {MaxNameLength} characters.");

    public static bool PromptConfirmation() =>
        InputHelper.PromptYesNo("Confirm", "Please enter only 'Y' for Yes and 'N' for No.");
}