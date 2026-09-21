using ECommerce.Domain.Entities;
using ECommerce.UseCases.Features.ProductBrands.Responses;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.ProductBrands.Specifications;
public sealed class BrandByIdToResponseSpecification : Specification<ProductBrand, BrandResponse>
{
    public BrandByIdToResponseSpecification(Guid id)
    {
        Query
            .Where(b => b.Id == id)
            .Select(b => new BrandResponse(b.Id, b.Name));
    }
}