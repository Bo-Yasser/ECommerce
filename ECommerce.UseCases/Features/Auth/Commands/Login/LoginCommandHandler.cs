using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Common.Interfaces;
using ECommerce.UseCases.Common.Options;
using ECommerce.UseCases.Features.Auth.Responses;
using MediatR;
using Microsoft.Extensions.Options;

namespace ECommerce.UseCases.Features.Auth.Commands.Login;

public sealed class LoginCommandHandler(
    IIdentityService identityService,
    IPendingRegistrationStore pendingStore,
    IJwtTokenGenerator jwtTokenGenerator,
    IRefreshTokenGenerator refreshTokenGenerator,
    IRefreshTokenRepository refreshTokenRepository,
    IOptions<JwtSettings> jwtOptions) 
    : IRequestHandler<LoginCommand, Result<AuthResponse>>
{
    private readonly JwtSettings _jwtSettings = jwtOptions.Value;

    public async Task<Result<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var email = request.Email.Trim();
        var password = request.Password.Trim();

        // Check if email is not confirmed
        var pendingUser = await pendingStore.GetAsync(email, cancellationToken);
        if (pendingUser is not null)
            return Result<AuthResponse>.Failure(AuthErrors.EmailNotConfirmed);


        // validate request
        var validatedResult = await identityService.ValidateCredentialsAsync(email, password, cancellationToken);

        if (validatedResult.IsFailure)
            return Result<AuthResponse>.Failure(validatedResult.Error!);

        var user = validatedResult.Value;

        // get user roles
        var roles = await identityService.GetRolesAsync(user.UserId, cancellationToken);

        // create Access token
        var accessToken = jwtTokenGenerator.GenerateToken(
            user.UserId,
            email,
            user.DisplayName,
            roles);

        // create string RefreshToken
        var refreshTokenString = refreshTokenGenerator.GenerateRefreshToken();

        // create RefreshToken entity
        var refreshTokenEntity = Domain.Entities.RefreshToken.Create(
            refreshTokenString,
            DateTimeOffset.UtcNow.AddDays(_jwtSettings.RefreshTokenExpirationDays),
            user.UserId);

        // limit sessions/devices
        var activeTokens = await refreshTokenRepository.GetActiveTokensForUserAsync(user.UserId, cancellationToken);

        if (activeTokens.Count >= _jwtSettings.MaxSessionsLimit)
        {
            int excessCount = activeTokens.Count - _jwtSettings.MaxSessionsLimit + 1;

            var tokensToRevoke = activeTokens.OrderBy(t => t.CreatedAt).Take(excessCount);

            foreach (var token in tokensToRevoke)
            {
                token.Revoke();
            }
        }

        // add RefreshToken to the database
        refreshTokenRepository.Add(refreshTokenEntity);
        await refreshTokenRepository.SaveChangesAsync(cancellationToken);

        var response = new AuthResponse(
            accessToken.AccessToken,
            refreshTokenString,
            accessToken.ExpireAtUtc);

        return Result<AuthResponse>.Success(response);
    }

}
