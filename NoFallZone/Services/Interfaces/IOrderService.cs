namespace NoFallZone.Services.Interfaces;
public interface IOrderService
{
    bool PlaceOrder(int shippingOptionId, string paymentMethod, out string message);
}