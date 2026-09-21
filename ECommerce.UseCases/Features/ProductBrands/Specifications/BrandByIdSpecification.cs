using ECommerce.Domain.Entities;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.ProductBrands.Specifications;

public sealed class BrandByIdSpecification : Specification<ProductBrand>
{
    public BrandByIdSpecification(Guid id)
        => Query.Where(b => b.Id == id);
}