using ECommerce.Domain.Common;
using ECommerce.Domain.Entities.StockAggregate;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Common.Pagination;
using ECommerce.UseCases.Features.Stocks.Responses;
using ECommerce.UseCases.Features.Stocks.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.Stocks.Queries.GetPagedStocks;

public sealed class GetPagedStocksQueryHandler(IReadRepository<Stock> repository)
    : IRequestHandler<GetPagedStocksQuery, Result<PagedResult<StockResponse>>>
{
    public async Task<Result<PagedResult<StockResponse>>> Handle(GetPagedStocksQuery request, CancellationToken cancellationToken)
    {
        var countSpec = new PagedStocksSpecification(
            search: request.Search,
            productId: request.ProductId,
            filter: request.Status);

        var listSpec = new PagedStocksSpecification(
            search: request.Search,
            productId: request.ProductId,
            filter: request.Status,
            sortBy: request.SortBy,
            sortDescending: request.SortDescending,
            pageNumber: request.PageNumber,
            pageSize: request.PageSize);

        var stocksCount = await repository.CountAsync(countSpec, cancellationToken);
        var stocks = await repository.ListAsync(listSpec, cancellationToken);

        return Result<PagedResult<StockResponse>>.Success(new PagedResult<StockResponse>(stocks, stocksCount));
    }
}
