using NoFallZone.Models.Entities;
using NoFallZone.Models.Enums;

namespace NoFallZone.Utilities.SessionManagement;
public static class Session
{
    public static Customer? LoggedInUser { get; set; }
    public static List<CartItem> Cart { get; } = new List<CartItem>();

    public static bool IsAdmin => LoggedInUser?.Role == Role.Admin;
    public static bool IsUser => LoggedInUser?.Role == Role.User;
    public static bool IsLoggedIn => LoggedInUser != null;

    public static void Logout()
    {
        LoggedInUser = null;
        Cart.Clear();
    }
    public static string GetDisplayNameAndRole() =>
    IsLoggedIn ? $"{LoggedInUser!.Username} ({LoggedInUser.Role})" : "Guest";

    public static decimal GetCartTotal() => Cart.Sum(i => i.Product.Price * i.Quantity);

}
