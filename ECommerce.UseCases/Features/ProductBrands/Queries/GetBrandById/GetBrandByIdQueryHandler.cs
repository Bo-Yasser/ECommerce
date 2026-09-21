using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Features.ProductBrands.Responses;
using ECommerce.UseCases.Features.ProductBrands.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.ProductBrands.Queries.GetBrandById;
public sealed class GetBrandByIdQueryHandler(
    IReadRepository<ProductBrand> repository)
    : IRequestHandler<GetBrandByIdQuery, Result<BrandResponse>>
{
    public async Task<Result<BrandResponse>> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
    {
        var brand = await repository.FirstOrDefaultAsync(
            new BrandByIdToResponseSpecification(request.Id),
            cancellationToken);

        if (brand is null)
            return Result<BrandResponse>.Failure(BrandErrors.NotFound);

        return Result<BrandResponse>.Success(brand);
    }
}
