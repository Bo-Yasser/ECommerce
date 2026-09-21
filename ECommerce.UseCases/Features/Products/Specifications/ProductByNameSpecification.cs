using ECommerce.Domain.Entities;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.Products.Specifications;

public sealed class ProductByNameSpecification : Specification<Product>
{
    public ProductByNameSpecification(string name, Guid? excludeId = null)
    {
        var trimmed = name?.Trim().ToLower();

        Query.Where(p => p.Name == trimmed);

        if (excludeId.HasValue && excludeId != Guid.Empty)
            Query.Where(p => p.Id != excludeId);
    }
}
