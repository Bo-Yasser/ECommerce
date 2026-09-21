using ECommerce.Domain.Entities;
using ECommerce.UseCases.Features.DeliveryMethods.Responses;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.DeliveryMethods.Specifications;

public sealed class DeliveryMethodByIdToResponseSpecification : Specification<DeliveryMethod, DeliveryMethodResponse>
{
    public DeliveryMethodByIdToResponseSpecification(Guid id)
    {
        Query
            .Where(dm => dm.Id == id)
            .Select(dm => new DeliveryMethodResponse
            (
                dm.Id,
                dm.Name,
                dm.Description,
                dm.Price,
                dm.EstimatedDeliveryTime,
                dm.IsAvailable,
                dm.DisplayOrder
            ));
    }
}
