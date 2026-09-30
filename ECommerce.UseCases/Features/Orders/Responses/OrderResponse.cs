using ECommerce.Domain.Entities.OrderAggregate;
using ECommerce.Domain.Enums;

namespace ECommerce.UseCases.Features.Orders.Responses;

public sealed record OrderResponse(
    Guid Id,
    Guid UserId,
    OrderStatus Status,
    Guid DeliveryMethodId,
    OrderDeliveryMethodResponse DeliveryMethod,
    ShippingAddressResponse ShippingAddress,
    decimal SubTotal,
    decimal ShippingCost,
    decimal Total,
    DateTimeOffset CreatedAt,
    IReadOnlyList<OrderItemResponse> OrderItems,
    byte[] RowVersion)
{
    public static OrderResponse From(Order order)
    {
        return new(
             order.Id,
             order.UserId,
             order.Status,
             order.DeliveryMethodId,
             OrderDeliveryMethodResponse.From(order.DeliveryMethod),
             ShippingAddressResponse.From(order.ShippingAddress),
             order.SubTotal,
             order.ShippingCost,
             order.Total,
             order.CreatedAt,
             order.Items.Select(OrderItemResponse.From).ToList(),
             order.RowVersion
        );
    }

}