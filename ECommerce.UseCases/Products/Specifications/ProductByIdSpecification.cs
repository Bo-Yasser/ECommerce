using ECommerce.Domain.Entities;
using ECommerce.UseCases.Products.Dtos;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Products.Specifications;

public sealed class ProductByIdSpecification : Specification<Product, GetByIdProductResponse>
{
    public ProductByIdSpecification(Guid productId)
    {
        Query
            .Where(p => p.Id == productId)
            .Select(product => new GetByIdProductResponse
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
