using ECommerce.Domain.Common;
using MediatR;

namespace ECommerce.UseCases.Features.ProductBrands.Commands.UpdateBrand;

public sealed record UpdateBrandCommand(Guid Id, string Name) : IRequest<Result>;