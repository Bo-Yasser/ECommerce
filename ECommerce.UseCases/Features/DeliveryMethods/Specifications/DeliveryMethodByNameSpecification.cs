using ECommerce.Domain.Entities;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.DeliveryMethods.Specifications;

public sealed class DeliveryMethodByNameSpecification : Specification<DeliveryMethod>
{
    public DeliveryMethodByNameSpecification(string name, Guid? excludeId = null)
    {
        var trimmed = name?.Trim().ToLower();

        Query.Where(dm => dm.Name == trimmed).AsTracking();

        if (excludeId.HasValue && excludeId != Guid.Empty)
            Query.Where(dm => dm.Id != excludeId);       
    }
}
