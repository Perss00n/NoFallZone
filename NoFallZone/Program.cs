using Microsoft.Extensions.DependencyInjection;
using NoFallZone.Data;
using NoFallZone.Menu;
using NoFallZone.Setup;

namespace NoFallZone;

public class Program
{
    static void Main(string[] args)
    {
        var services = new ServiceCollection();

        ServiceConfigurator.ConfigureServices(services);

        var provider = services.BuildServiceProvider();

        var db = provider.GetRequiredService<NoFallZoneContext>();
        var startPage = provider.GetRequiredService<StartPage>();

        var noFallZoneShop = new NoFallZoneApp(db, startPage);
        noFallZoneShop.Run();
    }
}