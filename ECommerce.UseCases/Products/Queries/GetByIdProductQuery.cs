using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Products.Dtos;
using ECommerce.UseCases.Products.Specifications;

namespace ECommerce.UseCases.Products.Queries;


public class GetByIdProductQuery(IReadRepository<Product> repository)
{
    public async Task<Result<GetByIdProductResponse>> ExecuteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await repository.FirstOrDefaultAsync(new ProductByIdSpecification(id), cancellationToken);
        
        if (product is null)
            return Result<GetByIdProductResponse>.Failure(ProductErrors.NotFound);
        
        return product;
    }
}