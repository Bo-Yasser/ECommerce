namespace ECommerce.UseCases.Features.Basket.Responses;

public sealed record BasketResponse(
    Guid BuyerId,
    IReadOnlyList<BasketItemResponse> Items,
    int TotalItems,
    decimal SubTotal,
    bool IsGuest)
{
    public static BasketResponse From(Domain.Entities.Basket basket, bool isGuest) =>
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
            basket.SubTotal,
            isGuest);
}
