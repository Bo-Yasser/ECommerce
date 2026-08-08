using ECommerce.Domain.Common;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.ProductTypes.Dtos;
using ECommerce.UseCases.ProductTypes.Specifications;

namespace ECommerce.UseCases.ProductTypes.Queries;

public class GetAllTypesQuery(IReadRepository<ProductType> repository)
{
    public async Task<Result<IReadOnlyList<GetAllTypesResponse>>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var types = await repository.ListAsync(new TypesListSpecification(), cancellationToken);
        return Result<IReadOnlyList<GetAllTypesResponse>>.Success(types);
    }
}
