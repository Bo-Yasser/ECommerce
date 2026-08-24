using ECommerce.Domain.Entities;
using ECommerce.UseCases.Features.ProductTypes.Responses;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.ProductTypes.Specifications;

public class TypesListSpecification : Specification<ProductType, GetTypesResponse>
{
    public TypesListSpecification()
    {
        Query
            .OrderBy(type => type.Name)
            .Select(type => new GetTypesResponse(type.Id, type.Name));
    }
}
