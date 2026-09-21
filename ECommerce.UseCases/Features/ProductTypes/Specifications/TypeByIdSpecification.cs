using ECommerce.Domain.Entities;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.ProductTypes.Specifications;

public sealed class TypeByIdSpecification : Specification<ProductType>
{
    public TypeByIdSpecification(Guid id)
        => Query.Where(t => t.Id == id);
}
