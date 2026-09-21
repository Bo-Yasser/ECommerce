using ECommerce.Domain.Common;
using MediatR;

namespace ECommerce.UseCases.Features.ProductTypes.Commands.UpdateType;
public sealed record UpdateTypeCommand(Guid Id, string Name) : IRequest<Result>;
