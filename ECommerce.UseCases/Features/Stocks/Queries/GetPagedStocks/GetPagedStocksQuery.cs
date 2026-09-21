using ECommerce.Domain.Common;
using ECommerce.UseCases.Common.Pagination;
using ECommerce.UseCases.Features.Stocks.Enums;
using ECommerce.UseCases.Features.Stocks.Responses;
using MediatR;

namespace ECommerce.UseCases.Features.Stocks.Queries.GetPagedStocks;

public sealed record GetPagedStocksQuery(
    int PageNumber = 1,
    int PageSize = 5,
    string? Search = null,
    Guid? ProductId = null,
    StockFilter Status = StockFilter.All,
    StockSortField SortBy = StockSortField.ProductName,
    bool SortDescending = false)
    : IRequest<Result<PagedResult<StockResponse>>>;
