namespace NoFallZone.Services.Interfaces
{
    public interface IPaymentOptionService
    {
        Task ShowAllPaymentOptionsAsync();
        Task AddPaymentOptionAsync();
        Task EditPaymentOptionAsync();
        Task DeletePaymentOptionAsync();
    }
}