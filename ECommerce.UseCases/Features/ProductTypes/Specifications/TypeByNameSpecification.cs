using ECommerce.Domain.Entities;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.ProductTypes.Specifications;

public sealed class TypeByNameSpecification : Specification<ProductType>
{
    public TypeByNameSpecification(string name, Guid? excludeId = null)
    {
        var trimmed = name?.Trim().ToLower();

        Query.Where(t => t.Name == trimmed).AsTracking();

        if (excludeId.HasValue && excludeId.Value != Guid.Empty)
            Query.Where(t => t.Id != excludeId.Value);
    }
}
