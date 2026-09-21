using ECommerce.Domain.Entities;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.Products.Specifications;

public sealed class ProductBrandSpecification : Specification<ProductBrand>
{
    public ProductBrandSpecification(Guid id)
        => Query.Where(b => b.Id == id);
}