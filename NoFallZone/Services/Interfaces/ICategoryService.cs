namespace NoFallZone.Services.Interfaces;
public interface ICategoryService
{
    Task ShowAllCategoriesAsync();
    Task AddCategoryAsync();
    Task EditCategoryAsync();
    Task DeleteCategoryAsync();
}
