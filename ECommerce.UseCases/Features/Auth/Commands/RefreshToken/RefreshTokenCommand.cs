using ECommerce.Domain.Common;
using ECommerce.UseCases.Features.Auth.Responses;
using MediatR;

namespace ECommerce.UseCases.Features.Auth.Commands.RefreshToken;
public sealed record RefreshTokenCommand(string Token) : IRequest<Result<AuthResponse>>;
