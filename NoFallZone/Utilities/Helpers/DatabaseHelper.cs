using Microsoft.EntityFrameworkCore;
using NoFallZone.Utilities.SessionManagement;

namespace NoFallZone.Utilities.Helpers;
public static class DatabaseHelper
{
    public static async Task<bool> TryToSaveToDbAsync(DbContext db)
    {
        try
        {
            await db.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException)
        {
            db.ChangeTracker.Clear();
            OutputHelper.ShowError("Database update failed! The input may have violated a database constraint.");
            return false;
        }
        catch (Exception ex)
        {
            db.ChangeTracker.Clear();
            if (Session.IsAdmin)
                OutputHelper.ShowError($"An unexpected error occured: {ex.Message}" + (ex.InnerException != null ? $"\nDetailed Information: {ex.InnerException.Message}" : ""));
            else
                OutputHelper.ShowError($"An unexpected error occured! Please contact an administrator if this error persists.");
            return false;
        }
    }
}
