using ECommerce.Domain.Entities;
using ECommerce.UseCases.Features.DeliveryMethods.Responses;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.DeliveryMethods.Specifications;

public sealed class DeliveryMethodsListSpecification : Specification<DeliveryMethod, DeliveryMethodResponse>
{
    public DeliveryMethodsListSpecification(bool availableOnly = false, string? search = null)
    {
        var query = Query;

        if (availableOnly)
            query.Where(dm => dm.IsAvailable);

        if (!string.IsNullOrWhiteSpace(search))
        {
            var term = search.Trim();

            query.Where(dm =>
                dm.Name.Contains(term) ||
                (dm.Description != null && dm.Description.Contains(term)) ||
                dm.EstimatedDeliveryTime.Contains(term));
        }

        query
            .OrderBy(dm => dm.DisplayOrder)
            .ThenBy(dm => dm.Name)
            .Select(dm => new DeliveryMethodResponse
            (
                dm.Id,
                dm.Name,
                dm.Description,
                dm.Price,
                dm.EstimatedDeliveryTime,
                dm.IsAvailable,
                dm.DisplayOrder,
                dm.RowVersion
            ));

    }
}
