using ECommerce.Domain.Common;
using MediatR;

namespace ECommerce.UseCases.Features.ProductTypes.Commands.DeleteType;

public sealed record DeleteTypeCommand(Guid Id) : IRequest<Result>;