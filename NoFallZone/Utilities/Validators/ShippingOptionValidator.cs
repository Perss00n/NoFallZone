using NoFallZone.Utilities.Helpers;

namespace NoFallZone.Utilities.Validators;
public static class ShippingOptionValidator
{
    public const int MinNameLength = 5;
    public const int MaxNameLength = 40;

    public const decimal MinPrice = 0;
    public const decimal MaxPrice = 1000;

    public static string PromptName() =>
        InputHelper.PromptRequiredLimitedString("Enter name of the shipping method (Ex 'Standard Shipping (3–5 days)')", MinNameLength, MaxNameLength,
            $"The name of the shipping method can't be empty and must be between {MinNameLength} and {MaxNameLength} chars!");

    public static decimal PromptPrice() =>
    InputHelper.PromptDecimal("Enter price for the shipping option", MinPrice, MaxPrice,
        $"Price must be between {MinPrice} and {MaxPrice}.");

    public static string? PromptOptionalName(string currentValue) =>
        InputHelper.PromptOptionalLimitedString($"Name [{currentValue}]", MinNameLength, MaxNameLength,
            $"The name of the shipping method must be between {MinNameLength} and {MaxNameLength} characters.");

    public static decimal? PromptOptionalPrice(decimal currentPrice) =>
    InputHelper.PromptOptionalDecimal($"Price [{currentPrice}]", MinPrice, MaxPrice,
        $"Price must be between {MinPrice} and {MaxPrice}.");

    public static bool PromptConfirmation() =>
        InputHelper.PromptYesNo("Confirm", "Please enter only 'Y' for Yes and 'N' for No.");
}
