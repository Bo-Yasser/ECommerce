using ECommerce.Domain.Entities.OrderAggregate;

namespace ECommerce.UseCases.Features.Orders.Responses;

public sealed record OrderDeliveryMethodResponse(
    string DeliveryMethodName,
    decimal DeliveryMethodPrice,
    string DeliveryMethodEstimatedTime)
{
    public static OrderDeliveryMethodResponse From(OrderDeliveryMethod deliveryMethod)
        => new(
            deliveryMethod.DeliveryMethodName,
            deliveryMethod.DeliveryMethodPrice,
            deliveryMethod.DeliveryMethodEstimatedTime);
}
