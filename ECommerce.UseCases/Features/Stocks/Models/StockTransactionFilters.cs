namespace ECommerce.UseCases.Features.Stocks.Models;

public sealed record StockTransactionFilters(
    string? Search = null,
    Guid? ReferenceId = null);
