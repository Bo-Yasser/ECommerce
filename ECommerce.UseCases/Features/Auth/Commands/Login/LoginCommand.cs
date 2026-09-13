using ECommerce.Domain.Common;
using ECommerce.UseCases.Features.Auth.Responses;
using MediatR;

namespace ECommerce.UseCases.Features.Auth.Commands.Login;

public sealed record LoginCommand(string Email, string Password) : IRequest<Result<AuthResponse>>;
