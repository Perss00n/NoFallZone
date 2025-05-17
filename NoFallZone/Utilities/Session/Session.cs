using NoFallZone.Models.Entities;
using NoFallZone.Models.Enums;

namespace NoFallZone.Utilities.Session;
public static class Session
{
    public static Customer? LoggedInUser { get; set; }

    public static bool IsAdmin => LoggedInUser?.Role == Role.Admin;
    public static bool IsUser => LoggedInUser?.Role == Role.User;
    public static bool IsLoggedIn => LoggedInUser != null;

    public static void Logout()
    {
        LoggedInUser = null;
    }
    public static string GetDisplayNameAndRole() =>
    IsLoggedIn ? $"{LoggedInUser!.Username} ({LoggedInUser.Role})" : "Guest";
}
