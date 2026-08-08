using ECommerce.Domain.Entities;
using ECommerce.UseCases.ProductTypes.Dtos;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.ProductTypes.Specifications;

public class TypesListSpecification : Specification<ProductType, GetAllTypesResponse>
{
    public TypesListSpecification()
    {
        Query
            .OrderBy(type => type.Name)
            .Select(type => new GetAllTypesResponse(type.Id, type.Name));
    }
}
