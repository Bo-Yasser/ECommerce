namespace ECommerce.Domain.Entities.OrderAggregate;

public sealed record OrderItemPayload(Guid ProductId, ProductItemOrdered Product, int Quantity);
