namespace ECommerce.UseCases.Features.Basket.Responses;

public record GetBasketResponse(
    Guid BuyerId,
    IReadOnlyList<BasketItemResponse> Items,
    int TotalItems,
    decimal SubTotal)
{
    public static GetBasketResponse From(Domain.Entities.Basket basket) =>
        new(
            basket.BuyerId,
            basket.Items
                .Select(item => new BasketItemResponse(
                    item.ProductId,
                    item.ProductName,
                    item.PictureUrl,
                    item.UnitPrice,
                    item.Quantity,
                    item.LineTotal))
                .ToList(),
            basket.TotalItems,
            basket.SubTotal);
}
