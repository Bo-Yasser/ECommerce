using ECommerce.Domain.Entities;
using ECommerce.UseCases.Features.Products.Responses;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.Products.Specifications;

public sealed class ProductsListSpecification : Specification<Product, GetProductsResponse>
{
    public ProductsListSpecification()
    {
        Query
            .OrderBy(product => product.Name)
            .Select(product => new GetProductsResponse
            (
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.PictureUrl,
                product.ProductType.Name,
                product.ProductBrand.Name
            ));
    }
}
