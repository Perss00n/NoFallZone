using NoFallZone.Models.Entities;

namespace NoFallZone.Services.Interfaces;

public interface ICartService
{
    int GetAvailableToAdd(Product product);
    bool TryAddToCart(Product product, int quantity, out string message);
    void OpenCartMenu();
    void ShowStartPageCartOverview();
    void AddDealToCart(ConsoleKey dealKey);
}