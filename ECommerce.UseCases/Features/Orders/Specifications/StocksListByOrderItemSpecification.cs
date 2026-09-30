using ECommerce.Domain.Entities.OrderAggregate;
using ECommerce.Domain.Entities.StockAggregate;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.Orders.Specifications;

public sealed class StocksListByOrderItemSpecification : Specification<Stock>
{
    public StocksListByOrderItemSpecification(IEnumerable<OrderItem> items)
    {
        var productIds = items
            .Select(i => i.ProductId)
            .ToHashSet();

        Query
            .Where(s => productIds.Contains(s.ProductId))
            .AsTracking();
    }
}
