using ECommerce.Domain.Entities.OrderAggregate;

namespace ECommerce.UseCases.Features.Orders.Responses;

public sealed record OrderItemResponse(
    Guid Id,
    Guid ProductId,
    ProductItemOrderedResponse ItemOrdered,
    int Quantity,
    decimal LineTotal)
{
    public static OrderItemResponse From(OrderItem item)
        => new(
            item.Id,
            item.ProductId,
            ProductItemOrderedResponse.From(item.ItemOrdered),
            item.Quantity,
            item.LineTotal);
}