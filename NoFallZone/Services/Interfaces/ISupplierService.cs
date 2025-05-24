namespace NoFallZone.Services.Interfaces;
public interface ISupplierService
{
    Task ShowAllSuppliersAsync();
    Task AddSupplierAsync();
    Task EditSupplierAsync();
    Task DeleteSupplierAsync();
}
