using ECommerce.Domain.Common;
using MediatR;

namespace ECommerce.UseCases.Features.ProductTypes.Commands.CreateType;

public sealed record CreateTypeCommand(string Name) : IRequest<Result<Guid>>;