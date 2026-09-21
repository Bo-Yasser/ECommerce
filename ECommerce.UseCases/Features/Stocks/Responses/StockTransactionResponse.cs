namespace ECommerce.UseCases.Features.Stocks.Responses;

public sealed record StockTransactionResponse(
    Guid Id,
    Guid StockId,
    int QuantityChanged,
    int QuantityBefore,
    int QuantityAfter,
    string Type,
    Guid? ReferenceId,
    string? Notes);