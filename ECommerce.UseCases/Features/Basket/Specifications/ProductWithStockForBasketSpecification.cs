using ECommerce.Domain.Entities;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.Basket.Specifications;

public sealed class ProductWithStockForBasketSpecification : Specification<Product>
{
    public ProductWithStockForBasketSpecification(Guid productId)
    {
        Query
            .Where(product => product.Id == productId)
            .Include(product => product.Stock);
    }
}