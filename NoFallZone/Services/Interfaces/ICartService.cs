using NoFallZone.Models.Entities;

namespace NoFallZone.Services.Interfaces;

public interface ICartService
{
    int GetAvailableToAdd(Product product);
    bool TryAddToCart(Product product, int quantity, out string message);
    Task OpenCartMenuAsync();
    void ShowStartPageCartOverview();
    Task AddDealToCartAsync(ConsoleKey dealKey);
}