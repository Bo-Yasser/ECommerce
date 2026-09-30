namespace ECommerce.Domain.Common.Errors;

public static class AuthErrors
{
    // Registration & OTP Flow
    public static readonly Error InvalidVerificationCode =
        Error.Validation(
            "Auth.InvalidVerificationCode",
            "Invalid or expired verification code.");

    public static readonly Error VerificationPending =
        Error.Conflict(
            "Auth.VerificationPending",
            "An account with this email is currently pending verification. Please check your inbox for the OTP.");

    public static readonly Error EmailSendFailed =
        Error.Failure(
            "Auth.EmailSendFailed",
            "Failed to send the verification email. Please try again later.");

    public static readonly Error OtpRequestLimitExceeded =
        Error.Failure(
            "Auth.OtpRequestLimitExceeded",
            "Too many verification requests! Please try registering again later.");

    // Login & Credentials
    public static readonly Error InvalidCredentials =
        Error.UnAuthorized(
            "Auth.InvalidCredentials",
            "Invalid email or password.");

    public static readonly Error EmailNotConfirmed =
        Error.Forbidden(
            "Auth.EmailNotConfirmed",
            "Email address has not been confirmed. Confirm your email before logging in.");

    public static readonly Error AccountLocked =
        Error.Forbidden(
            "Auth.AccountLocked",
            "Your account has been temporarily locked due to multiple failed login attempts. Please try again later.");

    // Token Management (JWT & Refresh)
    public static readonly Error InvalidRefreshToken =
        Error.UnAuthorized(
            "Auth.InvalidRefreshToken",
            "Invalid refresh token.");

    public static readonly Error RefreshTokenExpired =
        Error.UnAuthorized(
            "Auth.RefreshTokenExpired",
            "Refresh token has expired, Please sign in again.");

    public static readonly Error TokenMissing =
        Error.UnAuthorized(
            "Auth.TokenMissing",
            "Authentication token is missing from the request.");

    public static readonly Error InvalidAccessToken =
        Error.UnAuthorized(
            "Auth.InvalidAccessToken",
            "The provided access token is invalid or malformed.");

    public static readonly Error TokenExpired =
        Error.UnAuthorized(
            "Auth.TokenExpired",
            "The authentication token has expired. Please refresh your session.");

    public static readonly Error CompromisedSession =
        Error.UnAuthorized(
            "Auth.CompromisedSession",
            "A potential security issue was detected. For your protection, all active sessions have been terminated. Please log in again.");

    // Passwords & Recovery (Future-proofing)
    public static readonly Error InvalidPasswordResetToken =
        Error.Validation(
            "Auth.InvalidPasswordResetToken",
            "The password reset link is invalid or has expired.");

    public static readonly Error SameAsOldPassword =
        Error.Conflict(
            "Auth.SameAsOldPassword",
            "The new password cannot be the same as the current password.");

}