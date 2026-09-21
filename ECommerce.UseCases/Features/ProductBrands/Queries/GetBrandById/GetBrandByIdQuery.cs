using ECommerce.Domain.Common;
using ECommerce.UseCases.Features.ProductBrands.Responses;
using MediatR;

namespace ECommerce.UseCases.Features.ProductBrands.Queries.GetBrandById;

public sealed record GetBrandByIdQuery(Guid Id) : IRequest<Result<BrandResponse>>;