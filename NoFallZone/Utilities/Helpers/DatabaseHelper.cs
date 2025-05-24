using Microsoft.EntityFrameworkCore;
using NoFallZone.Utilities.SessionManagement;

namespace NoFallZone.Utilities.Helpers;
public static class DatabaseHelper
{
    public static bool TryToSaveToDb(DbContext db, out string errorMessage)
    {
        try
        {
            db.SaveChanges();
            errorMessage = string.Empty;
            return true;
        }
        catch (DbUpdateException)
        {
            db.ChangeTracker.Clear();
            errorMessage = "Database update failed! The input may have violated a database constraint.";
            return false;
        }
        catch (Exception ex)
        {
            db.ChangeTracker.Clear();
            if (Session.IsAdmin)
                errorMessage = $"An unexpected error occured: {ex.Message}" + (ex.InnerException != null ? $"\nDetailed Information: {ex.InnerException.Message}" : "");
            else
                errorMessage = $"An unexpected error occured! Please contact an administrator if this error persists.";
            return false;
        }
    }
}
