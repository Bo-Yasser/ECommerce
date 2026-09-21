using ECommerce.Domain.Entities;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.Products.Specifications;

public sealed class ProductTypeSpecification : Specification<ProductType>
{
    public ProductTypeSpecification(Guid id)
        => Query.Where(t => t.Id == id);
}
