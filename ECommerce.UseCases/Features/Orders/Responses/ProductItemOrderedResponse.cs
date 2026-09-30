using ECommerce.Domain.Entities.OrderAggregate;

namespace ECommerce.UseCases.Features.Orders.Responses;
public sealed record ProductItemOrderedResponse(
    string Sku,
    string ProductName,
    string PictureUrl,
    decimal UnitPrice)
{
    public static ProductItemOrderedResponse From(ProductItemOrdered item)
        => new(
            item.Sku,
            item.ProductName,
            item.PictureUrl,
            item.UnitPrice);
}