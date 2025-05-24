namespace NoFallZone.Services.Interfaces;
public interface IShippingOptionService
{
    Task ShowAllShippingOptionsAsync();
    Task AddShippingOptionAsync();
    Task EditShippingOptionAsync();
    Task DeleteShippingOptionAsync();
}
