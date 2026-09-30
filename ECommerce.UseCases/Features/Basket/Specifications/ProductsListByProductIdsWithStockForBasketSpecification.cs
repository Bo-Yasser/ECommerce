using ECommerce.Domain.Entities;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.Basket.Specifications;

public sealed class ProductsListByProductIdsWithStockForBasketSpecification : Specification<Product>
{
    public ProductsListByProductIdsWithStockForBasketSpecification(IReadOnlyList<Guid> productsIds) 
    { 
        Query
            .Where(p => productsIds.Contains(p.Id))
            .Include(p => p.Stock);
    } 
}