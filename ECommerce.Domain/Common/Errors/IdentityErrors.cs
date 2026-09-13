namespace ECommerce.Domain.Common.Errors;

public static class IdentityErrors
{
    public static readonly Error UserNotFound =
        Error.NotFound(
            "Identity.UserNotFound",
            "User was not found.");

    public static readonly Error EmailAlreadyExists =
        Error.Conflict(
            "Identity.EmailAlreadyExists",
            "Email is already registered.");

    public static readonly Error EmailAlreadyConfirmed =
        Error.Conflict(
            "Identity.EmailAlreadyConfirmed",
            "Email address is already confirmed.");

    public static readonly Error RoleNotFound =
        Error.NotFound(
            "Identity.RoleNotFound",
            "The specified role does not exist.");

    public static Error CreateFailed(string message) =>
        Error.Validation(
            "Identity.CreateFailed",
            message);

    public static Error UpdateFailed(string message) =>
        Error.Validation(
            "Identity.UpdateFailed",
            message);

    public static Error Validation(string message) =>
        Error.Validation(
            "Identity.Validation",
            message);
}