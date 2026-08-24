using ECommerce.Domain.Entities;
using ECommerce.UseCases.Features.Products.Responses;
using Mapster;

namespace ECommerce.UseCases.Profiles;

public class ProductConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Product, GetProductsResponse>()
            .Map(dest => dest.ProductBrand, src=> src.ProductBrand.Name)
            .Map(dest => dest.ProductType, src=> src.ProductType.Name);

        config.NewConfig<Product, GetProductByIdResponse>()
            .Map(dest => dest.ProductBrand, src => src.ProductBrand.Name)
            .Map(dest => dest.ProductType, src => src.ProductType.Name);
    }
}
