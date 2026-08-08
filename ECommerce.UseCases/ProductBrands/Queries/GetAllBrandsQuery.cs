using ECommerce.Domain.Common;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.ProductBrands.Dtos;
using ECommerce.UseCases.ProductBrands.Specifications;

namespace ECommerce.UseCases.ProductBrands.Queries;

public class GetAllBrandsQuery(IReadRepository<ProductBrand> repository)
{
    public async Task<Result<IReadOnlyList<GetAllBrandsResponse>>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var brands = await repository.ListAsync(new BrandsListSpecification(), cancellationToken);
        return Result<IReadOnlyList<GetAllBrandsResponse>>.Success(brands);
    }
}
