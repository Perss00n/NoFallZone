using NoFallZone.Data;
using NoFallZone.Models.Entities;
using NoFallZone.Utilities.SessionManagement;

namespace NoFallZone.Utilities.Helpers;
public static class LogHelper
{
    public static async Task LogAsync(NoFallZoneContext db, string action, string description)
    {
        if (!Session.IsLoggedIn) return;

        var log = new ActivityLog
        {
            Username = Session.LoggedInUser!.Username,
            Role = Session.LoggedInUser.Role.ToString(),
            Action = action,
            Timestamp = DateTime.Now,
            Details = description
        };

        await db.ActivityLogs.AddAsync(log);
        await db.SaveChangesAsync();
    }

    public static async Task LogAnonymousAsync(NoFallZoneContext db, string action, string description)
    {
        var log = new ActivityLog
        {
            Username = "Unknown",
            Role = "Unknown",
            Action = action,
            Timestamp = DateTime.Now,
            Details = description
        };

        await db.ActivityLogs.AddAsync(log);
        await db.SaveChangesAsync();
    }
}
