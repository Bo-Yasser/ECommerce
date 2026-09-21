namespace ECommerce.UseCases.Features.Basket.Responses;

public sealed record BasketItemResponse(
    Guid ProductId,
    string ProductName,
    string PictureUrl,
    decimal UnitPrice,
    int Quantity,
    decimal LineTotal);
