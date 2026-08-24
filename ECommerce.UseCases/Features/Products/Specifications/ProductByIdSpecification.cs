using ECommerce.Domain.Entities;
using ECommerce.UseCases.Features.Products.Responses;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.Products.Specifications;

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
