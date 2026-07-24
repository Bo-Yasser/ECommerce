using ECommerce.UseCases.ProductTypes.Dtos;

namespace ECommerce.UseCases.ProductTypes;

public interface IProductTypeQueryService
{
    Task<IReadOnlyList<GetAllTypesResponse>> GetAllTypesAsync(CancellationToken cancellationToken = default);

}
