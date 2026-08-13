using ECommerce.Domain.Entities;
using ECommerce.UseCases.Products.Responses;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Products.Specifications;

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
