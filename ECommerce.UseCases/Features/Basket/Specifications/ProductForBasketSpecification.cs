using ECommerce.Domain.Entities;
using ECommerce.UseCases.Features.Basket.Responses;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.Basket.Specifications;

public sealed class ProductForBasketSpecification : Specification<Product, ProductForBasketResponse>
{
    public ProductForBasketSpecification(Guid productId)
    {
        Query
            .Where(product => product.Id == productId)
            .Select(product => new ProductForBasketResponse(
                product.Id,
                product.Name,
                product.PictureUrl,
                product.Price));
    }
}