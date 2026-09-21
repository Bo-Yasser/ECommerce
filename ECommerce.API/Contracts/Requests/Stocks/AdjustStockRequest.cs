namespace ECommerce.API.Contracts.Requests.Stocks;

public sealed record AdjustStockRequest(int NewQuantity, string? Notes, byte[] RowVersion);
