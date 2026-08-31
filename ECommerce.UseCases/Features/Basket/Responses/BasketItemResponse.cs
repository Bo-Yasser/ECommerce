namespace ECommerce.UseCases.Features.Basket.Responses;

public record BasketItemResponse(
    Guid ProductId,
    string ProductName,
    string PictureUrl,
    decimal UnitPrice,
    int Quantity,
    decimal LineTotal);
