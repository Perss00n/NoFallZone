using NoFallZone.Data;
using NoFallZone.Menu;
using NoFallZone.Utilities.Helpers;
using NoFallZone.Utilities.SessionManagement;

namespace NoFallZone.Setup;

public class NoFallZoneApp
{
    private readonly NoFallZoneContext _db;
    private readonly StartPage _startPage;

    public NoFallZoneApp(NoFallZoneContext db, StartPage startPage)
    {
        _db = db;
        _startPage = startPage;
    }

    public void Run()
    {
        bool running = true;

        while (running)
        {
            Console.CursorVisible = false;
            DisplayHelper.ShowStartPage();

            var choice = Console.ReadKey(true).Key;

            if (choice == ConsoleKey.D3)
            {
                running = false;
                break;
            }

            bool isValidChoice = HandleInputs(choice);

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

    private bool HandleInputs(ConsoleKey input)
    {
        switch (input)
        {
            case ConsoleKey.D1:
                var user = LoginHelper.LoginUser(_db);
                if (user == null) return false;

                Session.LoggedInUser = user;
                _startPage.Show();
                return true;

            case ConsoleKey.D2:
                RegistrationHelper.RegisterNewCustomer(_db);
                if (Session.IsLoggedIn)
                    _startPage.Show();
                return true;

            default:
                return false;
        }
    }
}