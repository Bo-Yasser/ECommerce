using ECommerce.Domain.Entities.StockAggregate;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.Stocks.Specifications;

public sealed class StockByProductIdSpecification : Specification<Stock>
{
    public StockByProductIdSpecification(Guid productId)
    {
        Query
            .Where(s => s.ProductId == productId)
            .AsTracking();
    }
}
