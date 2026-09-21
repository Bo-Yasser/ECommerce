namespace ECommerce.UseCases.Features.DeliveryMethods.Responses;

public sealed record DeliveryMethodResponse(
    Guid Id,
    string Name,
    string? Description,
    decimal Price,
    string EstimatedDeliveryTime,
    bool IsAvailable,
    int DisplayOrder);
