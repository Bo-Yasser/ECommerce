namespace ECommerce.UseCases.Features.Stocks.Responses;
public sealed record StockResponse(
    Guid Id,
    Guid ProductId,
    string ProductSku,
    string ProductName,
    int Quantity,
    byte[] RowVersion);
