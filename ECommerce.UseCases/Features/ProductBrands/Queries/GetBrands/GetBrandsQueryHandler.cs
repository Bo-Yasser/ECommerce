using ECommerce.Domain.Common;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Features.ProductBrands.Responses;
using ECommerce.UseCases.Features.ProductBrands.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.ProductBrands.Queries.GetBrands;

public sealed class GetBrandsQueryHandler(IReadRepository<ProductBrand> repository) : IRequestHandler<GetBrandsQuery, Result<IReadOnlyList<BrandResponse>>>
{
    public async Task<Result<IReadOnlyList<BrandResponse>>> Handle(GetBrandsQuery request, CancellationToken cancellationToken)
    {
        var brands = await repository.ListAsync(
            new BrandsListSpecification(request.Search),
            cancellationToken);

        return Result<IReadOnlyList<BrandResponse>>.Success(brands);
    }
}
