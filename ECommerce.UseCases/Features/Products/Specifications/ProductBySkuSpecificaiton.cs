using ECommerce.Domain.Entities;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.Products.Specifications;

public sealed class ProductBySkuSpecificaiton : Specification<Product>
{
    public ProductBySkuSpecificaiton(string sku, Guid? excludeId = null)
    {
        var trimmed = sku.Trim();

        Query.Where(p => p.Sku == sku);

        if (excludeId.HasValue && excludeId != Guid.Empty)
            Query.Where(p => p.Id != excludeId);
    }
}