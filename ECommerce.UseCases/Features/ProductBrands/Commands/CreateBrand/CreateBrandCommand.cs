using ECommerce.Domain.Common;
using MediatR;

namespace ECommerce.UseCases.Features.ProductBrands.Commands.CreateBrand;

public sealed record CreateBrandCommand(string Name) : IRequest<Result<Guid>>;