using ECommerce.Domain.Entities;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.ProductBrands.Specifications;

public sealed class BrandByNameSpecification : Specification<ProductBrand>
{
    public BrandByNameSpecification(string name, Guid? excludeId = null)
    {
        var trimmed = name?.Trim().ToLower();

        Query.Where(b => b.Name == trimmed).AsTracking();

        if(excludeId.HasValue && excludeId != Guid.Empty)
            Query.Where(b => b.Id != excludeId);
    }
}
