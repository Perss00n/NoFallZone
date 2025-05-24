using Microsoft.Extensions.DependencyInjection;
using NoFallZone.Data;
using NoFallZone.Menu;
using NoFallZone.Setup;

class Program
{
    static async Task Main(string[] args)
    {
        var services = new ServiceCollection();

        ServiceConfigurator.ConfigureServices(services);

        var provider = services.BuildServiceProvider();

        var db = provider.GetRequiredService<NoFallZoneContext>();
        var startPage = provider.GetRequiredService<StartPage>();

        var noFallZoneShop = new NoFallZoneApp(db, startPage);
        await noFallZoneShop.RunAsync();
    }
}