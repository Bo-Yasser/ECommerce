using ECommerce.Domain.Common;
using MediatR;

namespace ECommerce.UseCases.Features.Auth.Commands.Register;

public sealed record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password) : IRequest<Result>;
