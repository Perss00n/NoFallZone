namespace NoFallZone.Services.Interfaces;
public interface IOrderService
{
    bool PlaceOrder(int shippingOptionId, int paymentMethodId, out string message);
}