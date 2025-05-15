using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoFallZone.Utilities;
public static class CategoryValidator
{

    public const int MinNameLength = 3;
    public const int MaxNameLength = 20;

    public static string PromptName() =>
    InputHelper.PromptRequiredLimitedString("Enter category name", MinNameLength, MaxNameLength,
        $"The category name can't be empty and must be between {MinNameLength} and {MaxNameLength} chars!");

    public static string? PromptOptionalName(string currentValue) =>
    InputHelper.PromptOptionalLimitedString($"Name [{currentValue}]", MinNameLength, MaxNameLength,
        $"Name must be between {MinNameLength} and {MaxNameLength} characters.");

    public static bool PromptConfirmation() =>
    InputHelper.PromptYesNo("Confirm", "Please enter only 'Y' for Yes and 'N' for No.");

}
