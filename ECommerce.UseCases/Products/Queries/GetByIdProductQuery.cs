using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.UseCases.Products.Dtos;

namespace ECommerce.UseCases.Products.Queries;


public class GetByIdProductQuery(IProductQueryService productQueryService)
{
    public async Task<Result<GetByIdProductResponse>> ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await productQueryService.GetByIdProductAsync(id, cancellationToken);
        if (product is null)
        {
            return Result<GetByIdProductResponse>.Failure(ProductErrors.NotFound);
        }
        return product;
    }
}