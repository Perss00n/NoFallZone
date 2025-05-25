namespace NoFallZone.Services.Interfaces;
public interface ICustomerService
{
    Task ShowAllCustomersAsync();
    Task AddCustomerAsync();
    Task EditCustomerAsync();
    Task DeleteCustomerAsync();
    Task ShowOrderHistoryAsync();
}
