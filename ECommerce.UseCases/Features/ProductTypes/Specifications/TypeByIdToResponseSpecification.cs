using ECommerce.Domain.Entities;
using ECommerce.UseCases.Features.ProductTypes.Responses;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.ProductTypes.Specifications;

public sealed class TypeByIdToResponseSpecification : Specification<ProductType, TypeResponse>
{
    public TypeByIdToResponseSpecification(Guid id)
    {
        Query
            .Where(t => t.Id == id)
            .Select(t => new TypeResponse(t.Id, t.Name));
    }
}
