using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFallZone.Models;

namespace NoFallZone.Utilities;
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
    IsLoggedIn ? $"{LoggedInUser!.FullName} ({LoggedInUser.Role})" : "Guest";
}
