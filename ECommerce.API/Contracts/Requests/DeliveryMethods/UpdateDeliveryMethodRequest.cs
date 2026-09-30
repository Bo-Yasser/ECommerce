namespace ECommerce.API.Contracts.Requests.DeliveryMethods;

public sealed record UpdateDeliveryMethodRequest(
    string Name,
    decimal Price,
    string EstimatedDeliveryTime,
    string? Description,
    bool IsAvailable,
    int DisplayOrder,
    byte[] RowVersion);
