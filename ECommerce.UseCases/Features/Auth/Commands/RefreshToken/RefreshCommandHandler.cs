using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Common.Interfaces;
using ECommerce.UseCases.Common.Options;
using ECommerce.UseCases.Features.Auth.Responses;
using MediatR;
using Microsoft.Extensions.Options;

namespace ECommerce.UseCases.Features.Auth.Commands.RefreshToken;

public sealed class RefreshCommandHandler(
    IIdentityService identityService,
    IJwtTokenGenerator jwtTokenGenerator,
    IRefreshTokenGenerator refreshTokenGenerator,
    IRefreshTokenRepository refreshTokenRepository,
    IOptions<JwtSettings> options) 
    : IRequestHandler<RefreshTokenCommand, Result<AuthResponse>>
{
    private readonly JwtSettings _jwtSettings = options.Value;
    public async Task<Result<AuthResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        // check if refresh token exists
        var existingToken = await refreshTokenRepository.GetByTokenAsync(request.Token.Trim(), cancellationToken);
        if (existingToken is null)
            return Result<AuthResponse>.Failure(AuthErrors.InvalidRefreshToken);

        // check if refresh token is revoked/used
        var userId = existingToken.UserId;
        if(existingToken.RevokedOn is not null)
        {
            var activeTokens = await refreshTokenRepository.GetActiveTokensForUserAsync(userId, cancellationToken);
            foreach(var token in activeTokens)
            {
                token.Revoke();
            }

            await refreshTokenRepository.SaveChangesAsync(cancellationToken);
            return Result<AuthResponse>.Failure(AuthErrors.CompromisedSession);
        }

        // check if refresh token expired
        if (existingToken.IsExpired)
            return Result<AuthResponse>.Failure(AuthErrors.RefreshTokenExpired);

        // revoke old refresh token
        existingToken.Revoke();

        // get the related user
        var userResult = await identityService.GetUserByIdAsync(userId, cancellationToken);
        if (userResult.IsFailure)
            return Result<AuthResponse>.Failure(IdentityErrors.UserNotFound);

        var user = userResult.Value;

        // get the user roles
        var roles = await identityService.GetRolesAsync(user.UserId, cancellationToken);

        // generate access token
        var accessToken = jwtTokenGenerator.GenerateToken(
            user.UserId,
            user.Email,
            user.DisplayName,
            roles);

        // generate refresh token
        var refreshTokenString = refreshTokenGenerator.GenerateRefreshToken();
        var refreshTokenEntity = Domain.Entities.RefreshToken.Create(
            refreshTokenString,
            DateTimeOffset.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
            user.UserId);

        // add refresh token to the database and change the old refresh token revoke state
        refreshTokenRepository.Add(refreshTokenEntity);
        await refreshTokenRepository.SaveChangesAsync(cancellationToken);

        var response = new AuthResponse(
            accessToken.AccessToken,
            refreshTokenString,
            accessToken.ExpireAtUtc);

        return Result<AuthResponse>.Success(response);
    }
}
