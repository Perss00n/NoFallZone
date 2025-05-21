using NoFallZone.Models.Entities;

namespace NoFallZone.Services.Interfaces;

public interface ICartService
{
    decimal GetCartTotal();
    int GetQuantityInCart(Product product);
    int GetAvailableToAdd(Product product);
    bool TryAddToCart(Product product, int quantity, out string message);
    bool RemoveFromCart(int index, out string message);
    bool ChangeQuantity(int index, int newQty, out string message);
    void OpenCartMenu();
    void ShowCartOverview();
    void AddDealToCart(ConsoleKey dealKey);
}