using ECommerce.Domain.Entities;
using ECommerce.UseCases.ProductBrands.Responses;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.ProductBrands.Specifications;

public sealed class BrandsListSpecification : Specification<ProductBrand, GetBrandsResponse>
{
    public BrandsListSpecification()
    {
        Query
            .OrderBy(brand => brand.Name)
            .Select(brand => new GetBrandsResponse(brand.Id, brand.Name));
    }
}
