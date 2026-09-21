using ECommerce.Domain.Common;
using MediatR;

namespace ECommerce.UseCases.Features.ProductBrands.Commands.DeleteBrand;

public sealed record DeleteBrandCommand(Guid Id) : IRequest<Result>;
