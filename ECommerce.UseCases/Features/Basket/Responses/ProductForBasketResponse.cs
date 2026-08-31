namespace ECommerce.UseCases.Features.Basket.Responses;

public sealed record ProductForBasketResponse(
    Guid Id,
    string Name,
    string PictureUrl,
    decimal Price);
