using ECommerce.Domain.Common;
using ECommerce.UseCases.ProductTypes.Dtos;

namespace ECommerce.UseCases.ProductTypes.Queries;

public class GetAllTypesQuery(IProductTypeQueryService productTypeQueryService)
{
    public async Task<Result<IReadOnlyList<GetAllTypesResponse>>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var types = await productTypeQueryService.GetAllTypesAsync(cancellationToken);
        return Result<IReadOnlyList<GetAllTypesResponse>>.Success(types);
    }
}
