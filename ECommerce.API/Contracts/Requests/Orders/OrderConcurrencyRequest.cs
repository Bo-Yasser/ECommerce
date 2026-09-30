namespace ECommerce.API.Contracts.Requests.Orders;

public sealed record OrderConcurrencyRequest(byte[] RowVersion);