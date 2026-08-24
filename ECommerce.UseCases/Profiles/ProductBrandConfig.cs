using ECommerce.Domain.Entities;
using ECommerce.UseCases.Features.ProductBrands.Responses;
using Mapster;

namespace ECommerce.UseCases.Profiles;

public class ProductBrandConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ProductBrand, GetBrandsResponse>();
    }
}
