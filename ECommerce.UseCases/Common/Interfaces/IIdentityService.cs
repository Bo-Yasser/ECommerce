using ECommerce.Domain.Common;
using ECommerce.UseCases.Common.Models;

namespace ECommerce.UseCases.Common.Interfaces;

public interface IIdentityService
{
    string HashPassword(string plainPassword);

    Task<Result<IdentityUserInfo>> CreateVerifiedUserAsync(
        string email,
        string preHashedPassword,
        string? displayName,
        CancellationToken cancellationToken = default);

    Task<Result<IdentityUserInfo>> ValidateCredentialsAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default);

    Task<Result<IdentityUserInfo>> GetUserByEmailAsync( 
        string email,
        CancellationToken cancellationToken = default);

    Task<Result<IdentityUserInfo>> GetUserByIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<Result<IdentityUserInfo>> UpdateProfileAsync(
        Guid userId,
        string? displayName,
        CancellationToken cancellationToken = default);

    Task<bool> IsEmailConfirmedAsync(
        string email,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<string>> GetRolesAsync(
        Guid userId,
        CancellationToken cancellationToken = default);

    Task<bool> IsEmailExists(string email, CancellationToken cancellationToken = default);
}
