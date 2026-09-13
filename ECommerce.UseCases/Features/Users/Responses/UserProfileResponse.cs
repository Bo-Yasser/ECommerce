namespace ECommerce.UseCases.Features.Users.Responses;

public sealed record UserProfileResponse(
    Guid UserId,
    string Email,
    string? DisplayName);
