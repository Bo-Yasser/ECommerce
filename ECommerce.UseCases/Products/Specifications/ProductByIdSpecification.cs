using ECommerce.Domain.Entities;
using ECommerce.UseCases.Products.Responses;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Products.Specifications;

public sealed class ProductByIdSpecification : Specification<Product, GetProductByIdResponse>
{
    public ProductByIdSpecification(Guid productId)
    {
        Query
            .Where(p => p.Id == productId)
            .Select(product => new GetProductByIdResponse
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
