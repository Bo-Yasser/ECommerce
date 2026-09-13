using ECommerce.Domain.Common;
using MediatR;

namespace ECommerce.UseCases.Features.Auth.Commands.RevokeToken;

public sealed record RevokeTokenCommand(string Token) : IRequest<Result>;
