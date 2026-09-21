using ECommerce.Domain.Entities;
using ECommerce.UseCases.Features.ProductBrands.Responses;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.ProductBrands.Specifications;

public sealed class BrandsListSpecification : Specification<ProductBrand, BrandResponse>
{
    public BrandsListSpecification(string? search = null)
    {
        var query = Query;

        if (!string.IsNullOrWhiteSpace(search))
            query.Where(type => type.Name.Contains(search.Trim()));

        query
            .OrderBy(brand => brand.Name)
            .Select(brand => new BrandResponse(brand.Id, brand.Name));
    }
}
