namespace NoFallZone.Services.Interfaces
{
    public interface IPaymentOptionService
    {
        void ShowAllPaymentOptions();
        void AddPaymentOption();
        void EditPaymentOption();
        void DeletePaymentOption();
    }
}