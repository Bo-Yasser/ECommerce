using ECommerce.UseCases.Features.Stocks.Enums;
using ECommerce.UseCases.Features.Stocks.Models;

namespace ECommerce.API.Contracts.Requests.Stocks;

public sealed record GetPagedStockTransactionsRequest(
    int PageNumber = 1,
    int PageSize = 5,
    StockTransactionFilters? Filters = null,
    StockTransactionSortField SortBy = StockTransactionSortField.Reference,
    bool SortDescending = false);