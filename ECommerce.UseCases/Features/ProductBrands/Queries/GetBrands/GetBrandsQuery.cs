using ECommerce.Domain.Common;
using ECommerce.UseCases.Features.ProductBrands.Responses;
using MediatR;

namespace ECommerce.UseCases.Features.ProductBrands.Queries.GetBrands;

public sealed record GetBrandsQuery : IRequest<Result<IReadOnlyList<GetBrandsResponse>>>;
