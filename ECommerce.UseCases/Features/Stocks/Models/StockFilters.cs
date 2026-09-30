using ECommerce.UseCases.Features.Stocks.Enums;
namespace ECommerce.UseCases.Features.Stocks.Models;


public sealed record StockFilters(
    string? Search = null,
    Guid? ProductId = null,
    StockAvailabilityFilter AvailabilityFilter = StockAvailabilityFilter.All);