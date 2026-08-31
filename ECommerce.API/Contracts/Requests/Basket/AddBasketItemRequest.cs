namespace ECommerce.API.Contracts.Requests.Basket;

public sealed record AddBasketItemRequest(Guid ProductId, int Quantity);
