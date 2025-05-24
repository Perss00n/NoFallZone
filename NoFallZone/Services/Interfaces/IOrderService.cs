using NoFallZone.Models.Entities;

namespace NoFallZone.Services.Interfaces;
public interface IOrderService
{
    Task<bool> PlaceOrderAsync(int shippingOptionId, int paymentMethodId);
    void ShowOrderPreview(ShippingOption shipping, PaymentOption payment);
}