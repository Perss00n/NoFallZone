using NoFallZone.Data;
using NoFallZone.Menu;
using NoFallZone.Utilities.Helpers;
using NoFallZone.Utilities.SessionManagement;

public class NoFallZoneApp
{
    private readonly NoFallZoneContext _db;
    private readonly StartPage _startPage;

    public NoFallZoneApp(NoFallZoneContext db, StartPage startPage)
    {
        _db = db;
        _startPage = startPage;
    }

    public async Task RunAsync()
    {
        bool running = true;

        while (running)
        {
            Console.CursorVisible = false;
            Console.WriteLine(DisplayHelper.ShowLogo());
            DisplayHelper.ShowStartPage();
            DisplayHelper.ShowSetupPage();

            var choice = Console.ReadKey(true).Key;

            if (choice == ConsoleKey.D3)
            {
                running = false;
                break;
            }

            bool isValidChoice = await HandleInputsAsync(choice);

            if (!isValidChoice)
            {
                Console.Clear();
                Console.WriteLine(DisplayHelper.ShowLogo());
                OutputHelper.ShowError("Invalid choice!");
            }

            OutputHelper.ShowInfo("Press any key to continue...");
            Console.ReadKey();
        }

        Console.Clear();
        Console.WriteLine(DisplayHelper.ShowLogo());
        OutputHelper.ShowInfo("".PadRight(35) + "Thank you for visiting NoFallZone! L8terZ!");
    }

    private async Task<bool> HandleInputsAsync(ConsoleKey input)
    {
        switch (input)
        {
            case ConsoleKey.D1:
                var user = await LoginHelper.LoginUserAsync(_db);
                if (user == null) return false;

                Session.LoggedInUser = user;
                await _startPage.ShowAsync();
                return true;

            case ConsoleKey.D2:
                await RegistrationHelper.RegisterNewCustomerAsync(_db);
                if (Session.IsLoggedIn)
                    await _startPage.ShowAsync();
                return true;

            case ConsoleKey.P:
                await SeedData.InitializeAsync(_db);
                return true;

            case ConsoleKey.C:
                await SeedData.ClearDatabaseAsync(_db);
                return true;

            default:
                return false;
        }
    }
}