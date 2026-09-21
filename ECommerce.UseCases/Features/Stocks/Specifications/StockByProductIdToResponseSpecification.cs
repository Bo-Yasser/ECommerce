using ECommerce.Domain.Entities.StockAggregate;
using ECommerce.UseCases.Features.Stocks.Responses;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.Stocks.Specifications;

public sealed class StockByProductIdToResponseSpecification : Specification<Stock, StockResponse>
{
    public StockByProductIdToResponseSpecification(Guid productId)
    {
        Query
            .Where(s => s.ProductId == productId)
            .Select(s => new StockResponse(
                s.Id,
                s.ProductId,
                s.Product.Sku,
                s.Product.Name,
                s.Quantity,
                s.RowVersion));
    }
}
