using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoFallZone.Utilities;
public static class ProductValidator
{
    public const int MinNameLength = 2;
    public const int MaxNameLength = 50;

    public const int MinDescriptionLength = 5;
    public const int MaxDescriptionLength = 200;

    public const decimal MinPrice = 1m;
    public const decimal MaxPrice = 50000m;

    public const int MinStock = 0;
    public const int MaxStock = 10000;

    public static string PromptName() =>
        InputHelper.PromptRequiredLimitedString("Enter product name", MinNameLength, MaxNameLength,
            $"Product name must be between {MinNameLength} and {MaxNameLength} characters.");

    public static string PromptDescription() =>
        InputHelper.PromptRequiredLimitedString("Enter product description", MinDescriptionLength, MaxDescriptionLength,
            $"Description must be between {MinDescriptionLength} and {MaxDescriptionLength} characters.");

    public static decimal PromptPrice() =>
        InputHelper.PromptDecimal("Enter price", MinPrice, MaxPrice,
            $"Price must be between {MinPrice} and {MaxPrice}.");

    public static int PromptStock() =>
        InputHelper.PromptInt("Enter stock quantity", MinStock, MaxStock,
            $"Stock must be between {MinStock} and {MaxStock}.");

    public static string? PromptOptionalName(string currentValue) =>
        InputHelper.PromptOptionalLimitedString($"Name [{currentValue}]", MinNameLength, MaxNameLength,
            $"Name must be between {MinNameLength} and {MaxNameLength} characters.");

    public static string? PromptOptionalDescription(string currentValue) =>
        InputHelper.PromptOptionalLimitedString($"Description [{currentValue}]", MinDescriptionLength, MaxDescriptionLength,
            $"Description must be between {MinDescriptionLength} and {MaxDescriptionLength} characters.");

    public static decimal? PromptOptionalPrice(decimal currentPrice) =>
        InputHelper.PromptOptionalDecimal($"Price [{currentPrice}]", MinPrice, MaxPrice,
            $"Price must be between {MinPrice} and {MaxPrice}.");

    public static int? PromptOptionalStock(int currentStock) =>
        InputHelper.PromptOptionalInt($"Stock [{currentStock}]", MinStock, MaxStock,
            $"Stock must be between {MinStock} and {MaxStock}.");
    public static bool PromptConfirmation() =>
        InputHelper.PromptYesNo("Confirm", "Please enter only 'Y' for Yes and 'N' for No.");
}
