using ECommerce.Domain.Entities;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.Orders.Specifications;

public sealed class DeliveryMethodByIdSpecification : Specification<DeliveryMethod>
{
    public DeliveryMethodByIdSpecification(Guid deliveryMethodId)
        => Query.Where(dm => dm.Id == deliveryMethodId && dm.IsAvailable);
}
