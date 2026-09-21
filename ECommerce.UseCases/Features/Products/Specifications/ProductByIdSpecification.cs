using ECommerce.Domain.Entities;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.Products.Specifications;

public sealed class ProductByIdSpecification : Specification<Product>
{
    public ProductByIdSpecification(Guid id)
        => Query.Where(p => p.Id == id).AsTracking();
}
