using ECommerce.Domain.Common;
using ECommerce.UseCases.ProductBrands.Responses;
using MediatR;

namespace ECommerce.UseCases.ProductBrands.Queries.GetBrands;

public sealed record GetBrandsQuery : IRequest<Result<IReadOnlyList<GetBrandsResponse>>>;
