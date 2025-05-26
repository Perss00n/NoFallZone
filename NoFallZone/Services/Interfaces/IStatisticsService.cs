namespace NoFallZone.Services.Interfaces;
public interface IStatisticsService
{
    Task ShowMostSoldProductsAsync();
    Task ShowTopCategoriesAsync();
    Task ShowDealSalesCountAsync();
    Task ShowOrdersByCityAsync();
    Task ShowSalesBySupplierAsync();
    Task ShowTopSearchKeywordsAsync();
}
