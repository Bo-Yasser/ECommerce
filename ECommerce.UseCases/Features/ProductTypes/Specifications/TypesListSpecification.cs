using ECommerce.Domain.Entities;
using ECommerce.UseCases.Features.ProductTypes.Responses;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.ProductTypes.Specifications;

public sealed class TypesListSpecification : Specification<ProductType, TypeResponse>
{
    public TypesListSpecification(string? search = null)
    {
        var query = Query;

        if (!string.IsNullOrWhiteSpace(search))
            query.Where(type => type.Name.Contains(search.Trim()));

        query
            .OrderBy(type => type.Name)
            .Select(type => new TypeResponse(type.Id, type.Name));
    }
}
