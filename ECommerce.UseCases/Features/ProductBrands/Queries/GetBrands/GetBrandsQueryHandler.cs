using ECommerce.Domain.Common;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Features.ProductBrands.Responses;
using ECommerce.UseCases.Features.ProductBrands.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.ProductBrands.Queries.GetBrands;

public class GetBrandsQueryHandler(IReadRepository<ProductBrand> repository) : IRequestHandler<GetBrandsQuery, Result<IReadOnlyList<GetBrandsResponse>>>
{
    public async Task<Result<IReadOnlyList<GetBrandsResponse>>> Handle(GetBrandsQuery request, CancellationToken cancellationToken)
    {
        var brands = await repository.ListAsync(new BrandsListSpecification(), cancellationToken);
        return Result<IReadOnlyList<GetBrandsResponse>>.Success(brands);
    }
}
