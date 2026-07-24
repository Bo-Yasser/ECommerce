using ECommerce.Domain.Entities;
using ECommerce.UseCases.ProductBrands.Dtos;
using Mapster;

namespace ECommerce.UseCases.Profiles;

public class ProductBrandConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ProductBrand, GetAllBrandsResponse>();
    }
}
