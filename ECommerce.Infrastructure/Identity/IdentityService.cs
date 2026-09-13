using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Constants;
using ECommerce.UseCases.Common.Interfaces;
using ECommerce.UseCases.Common.Models;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Infrastructure.Identity;

public class IdentityService(UserManager<ApplicationUser> userManager) : IIdentityService
{
    public string HashPassword(string plainPassword)
    {
        var dummyUser = new ApplicationUser();
        return userManager.PasswordHasher.HashPassword(dummyUser, plainPassword);
    }
    public async Task<Result<IdentityUserInfo>> CreateVerifiedUserAsync(
        string email, 
        string preHashedPassword, 
        string? displayName,
        CancellationToken cancellationToken = default)
    {
        if (await IsEmailExists(email, cancellationToken))
        {
            return Result<IdentityUserInfo>.Failure(IdentityErrors.EmailAlreadyExists);
        }

        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            DisplayName = displayName,
            PasswordHash = preHashedPassword,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user);
        if (!result.Succeeded)
        {
            if (result.Errors.Any(e => e.Code is "DuplicateEmail" or "DuplicateUserName"))
            {
                return Result<IdentityUserInfo>.Failure(IdentityErrors.EmailAlreadyExists);
            }

            var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result<IdentityUserInfo>.Failure(IdentityErrors.CreateFailed(errorMessage));
        }

        await userManager.AddToRoleAsync(user, Roles.User);
        return Result<IdentityUserInfo>.Success(
            new IdentityUserInfo(user.Id, user.Email, user.DisplayName));

    }

    public async Task<Result<IdentityUserInfo>> ValidateCredentialsAsync(
        string email,
        string password,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if(user is null)
            return Result<IdentityUserInfo>.Failure(AuthErrors.InvalidCredentials);

        var isValid = await userManager.CheckPasswordAsync(user, password);
        if(!isValid)
            return Result<IdentityUserInfo>.Failure(AuthErrors.InvalidCredentials);

        if(!user.EmailConfirmed)
            return Result<IdentityUserInfo>.Failure(AuthErrors.EmailNotConfirmed);

        return Result<IdentityUserInfo>.Success(
            new IdentityUserInfo(user.Id, user.Email!, user.DisplayName));

    }
    public async Task<Result<IdentityUserInfo>> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user is null)
            return Result<IdentityUserInfo>.Failure(IdentityErrors.UserNotFound);

        return Result<IdentityUserInfo>.Success(
            new IdentityUserInfo(user.Id, user.Email!, user.DisplayName));
    }

    public async Task<Result<IdentityUserInfo>> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return Result<IdentityUserInfo>.Failure(IdentityErrors.UserNotFound);

        return Result<IdentityUserInfo>.Success(
            new IdentityUserInfo(user.Id, user.Email!, user.DisplayName));
    }

    public async Task<Result<IdentityUserInfo>> UpdateProfileAsync(
        Guid userId,
        string? displayName,
        CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if(user is null)
            return Result<IdentityUserInfo>.Failure(IdentityErrors.UserNotFound);

        user.DisplayName = string.IsNullOrWhiteSpace(displayName) ? user.DisplayName : displayName.Trim();
        var result = await userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            var message = string.Join(", ", result.Errors.Select(e => e.Description));
            return Result<IdentityUserInfo>.Failure(IdentityErrors.CreateFailed(message));
        }
        
        return Result<IdentityUserInfo>.Success(
            new IdentityUserInfo(user.Id, user.Email!, user.DisplayName));
    }

    public async Task<bool> IsEmailConfirmedAsync(string email, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        return user?.EmailConfirmed ?? false;
    }

    public async Task<IReadOnlyList<string>> GetRolesAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByIdAsync(userId.ToString());
        if (user is null)
            return [];

        var roles = await userManager.GetRolesAsync(user);
        return roles.ToList();
    }

    public async Task<bool> IsEmailExists(string email, CancellationToken cancellationToken = default)
    {
        var user = await userManager.FindByEmailAsync(email);
        return user is not null;
    }

}
