using Microsoft.Extensions.DependencyInjection;
using NoFallZone.Data;
using NoFallZone.Menu;
using NoFallZone.Services.Implementations;
using NoFallZone.Services.Interfaces;

namespace NoFallZone.Setup;

public static class ServiceConfigurator
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<NoFallZoneContext>();

        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<ISupplierService, SupplierService>();
        services.AddScoped<ICartService, CartService>();


        services.AddScoped<AdminMenu>();
        services.AddScoped<CustomerMenu>();
        services.AddScoped<StartPage>();
    }
}