using ECommerce.Domain.Common;
using ECommerce.UseCases.Products.Dtos;

namespace ECommerce.UseCases.Products.Queries;

public class GetAllProductsQuery(IProductQueryService productQueryService)
{
    public async Task<Result<IReadOnlyList<GetAllProductsResponse>>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var products = await productQueryService.GetAllProductsAsync(cancellationToken);
        return Result<IReadOnlyList<GetAllProductsResponse>>.Success(products);
    }
}
