using ECommerce.Domain.Entities;
using ECommerce.UseCases.Features.ProductTypes.Responses;
using Mapster;

namespace ECommerce.UseCases.Profiles;

public class ProductTypeConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<ProductType, GetTypesResponse>();
    }
}
