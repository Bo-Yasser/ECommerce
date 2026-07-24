using ECommerce.UseCases.ProductBrands.Dtos;

namespace ECommerce.UseCases.ProductBrands;

public interface IProductBrandQueryService
{
    Task<IReadOnlyList<GetAllBrandsResponse>> GetAllBrandsAsync(CancellationToken cancellationToken = default);
}
