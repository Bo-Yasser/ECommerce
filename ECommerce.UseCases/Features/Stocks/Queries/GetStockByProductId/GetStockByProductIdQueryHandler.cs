using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Entities.StockAggregate;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Features.Stocks.Responses;
using ECommerce.UseCases.Features.Stocks.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.Stocks.Queries.GetStockByProductId;

public sealed class GetStockByProductIdQueryHandler(IReadRepository<Stock> repository)
    : IRequestHandler<GetStockByProductIdQuery, Result<StockResponse>>
{
    public async Task<Result<StockResponse>> Handle(GetStockByProductIdQuery request, CancellationToken cancellationToken)
    {
        var stock = await repository.FirstOrDefaultAsync(
            new StockByProductIdToResponseSpecification(request.ProductId),
            cancellationToken);

        if (stock is null)
            return Result<StockResponse>.Failure(StockErrors.NotFound);

        return Result<StockResponse>.Success(stock);
    }
}
