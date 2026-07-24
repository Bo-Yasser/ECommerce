using ECommerce.Domain.Common;
using ECommerce.UseCases.ProductBrands.Dtos;

namespace ECommerce.UseCases.ProductBrands.Queries;

public class GetAllBrandsQuery(IProductBrandQueryService prodcutBrandQueryService)
{
    public async Task<Result<IReadOnlyList<GetAllBrandsResponse>>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var brands = await prodcutBrandQueryService.GetAllBrandsAsync(cancellationToken);
        return Result<IReadOnlyList<GetAllBrandsResponse>>.Success(brands);
    }
}
