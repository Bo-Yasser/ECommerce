using ECommerce.Domain.Common;
using ECommerce.Domain.Entities.StockAggregate;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Common.Pagination;
using ECommerce.UseCases.Features.Stocks.Responses;
using ECommerce.UseCases.Features.Stocks.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.Stocks.Queries.GetPagedStockTransactions;

public sealed class GetPagedStockTransactionsQueryHandler(
    IReadRepository<StockTransaction> repository) : IRequestHandler<GetPagedStockTransactionsQuery, Result<PagedResult<StockTransactionResponse>>>
{
    public async Task<Result<PagedResult<StockTransactionResponse>>> Handle(GetPagedStockTransactionsQuery request, CancellationToken cancellationToken)
    {
        var countSpec = new PagedStockTransactionsByProductIdSpecification(
            productId: request.ProductId,
            search: request.Search,
            referenceId: request.ReferenceId);

        var listSpec = new PagedStockTransactionsByProductIdSpecification(
            productId: request.ProductId,
            search: request.Search,
            referenceId: request.ReferenceId,
            sortBy: request.SortBy,
            sortDescending:request.SortDescending,
            pageNumber: request.PageNumber,
            pageSize: request.PageSize);

        var transactionsCount = await repository.CountAsync(countSpec, cancellationToken);
        var transactions = await repository.ListAsync(listSpec, cancellationToken);

        return Result<PagedResult<StockTransactionResponse>>.Success(new PagedResult<StockTransactionResponse>(transactions, transactionsCount));
    }
}
