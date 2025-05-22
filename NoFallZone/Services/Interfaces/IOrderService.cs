using NoFallZone.Models.Entities;

namespace NoFallZone.Services.Interfaces;
public interface IOrderService
{
    bool PlaceOrder(int shippingOptionId, int paymentMethodId, out string message);

    void ShowOrderPreview(ShippingOption shipping, PaymentOption payment);
}