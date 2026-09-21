using ECommerce.Domain.Entities;
using ECommerce.UseCases.Features.Products.Responses;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.Products.Specifications;

public sealed class ProductByIdToResponseSpecification : Specification<Product, ProductResponse>
{
    public ProductByIdToResponseSpecification(Guid productId)
    {
        Query
            .Where(p => p.Id == productId)
            .Select(product => new ProductResponse
            (
                product.Id,
                product.Sku,
                product.Name,
                product.Description,
                product.Price,
                product.PictureUrl,
                product.ProductType.Name,
                product.ProductBrand.Name,
                product.Stock.Quantity,
                product.Stock.Quantity > 0
            ));
    }
}
