using ECommerce.Domain.Common;
using ECommerce.Domain.Repositories;
using MediatR;

namespace ECommerce.UseCases.Features.Auth.Commands.RevokeToken;

public sealed class RevokeTokenCommandHandler(IRefreshTokenRepository refreshTokenRepository) 
    : IRequestHandler<RevokeTokenCommand, Result>
{
    public async Task<Result> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        var existingToken = await refreshTokenRepository.GetByTokenAsync(request.Token.Trim(), cancellationToken);
        if (existingToken is null || !existingToken.IsActive)
            return Result.Success();

        existingToken.Revoke();
        await refreshTokenRepository.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
