using ECommerce.Domain.Common;
using ECommerce.UseCases.Common.Pagination;
using ECommerce.UseCases.Features.Stocks.Enums;
using ECommerce.UseCases.Features.Stocks.Responses;
using MediatR;

namespace ECommerce.UseCases.Features.Stocks.Queries.GetPagedStockTransactions;

public sealed record GetPagedStockTransactionsQuery(
    Guid ProductId,
    int PageNumber = 1,
    int PageSize = 5,
    string? Search = null,
    Guid? ReferenceId = null,
    StockTransactionSortField SortBy = StockTransactionSortField.Reference,
    bool SortDescending = false)
    : IRequest<Result<PagedResult<StockTransactionResponse>>>;
