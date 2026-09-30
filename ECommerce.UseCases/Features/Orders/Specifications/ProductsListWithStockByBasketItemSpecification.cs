using ECommerce.Domain.Entities;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.Orders.Specifications;

public sealed class ProductsListWithStockByBasketItemSpecification : Specification<Product>
{
    public ProductsListWithStockByBasketItemSpecification(IEnumerable<BasketItem> items)
    {
        var productIds = items
            .Select(i => i.ProductId)
            .ToHashSet();

        Query.Where(p => productIds.Contains(p.Id))
            .Include(p => p.Stock)
            .AsTracking();
    }
}