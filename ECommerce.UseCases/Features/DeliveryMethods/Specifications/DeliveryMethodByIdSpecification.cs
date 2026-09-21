using ECommerce.Domain.Entities;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.DeliveryMethods.Specifications;
public sealed class DeliveryMethodByIdSpecification : Specification<DeliveryMethod>
{
    public DeliveryMethodByIdSpecification(Guid id)
        => Query.Where(dm => dm.Id == id).AsTracking();
}
