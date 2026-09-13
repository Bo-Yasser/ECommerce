
namespace ECommerce.API.Constants;

public static class AuthMessages
{
    // 1. Registration & OTP Flow (Current)
    public const string RegistrationInitiatedSuccessfully = "Registration initiated successfully. Please check your email for the verification code.";
    public const string EmailConfirmedSuccessfully = "Email confirmed successfully. Your account is now active and ready to use.";
    public const string OtpResentSuccessfully = "A new verification code has been sent to your email address.";

    // 2. Login & Token Management
    public const string LoginSuccessful = "Logged in successfully.";
    public const string TokenRefreshedSuccessfully = "Authentication token refreshed successfully.";
    public const string LogoutSuccessful = "Logged out successfully.";

    // 3. Password Management & Recovery (Future)
    public const string PasswordResetLinkSent = "If an account matches the provided email, password reset instructions have been sent.";
    public const string PasswordResetSuccessfully = "Your password has been reset successfully. You can now log in with your new password.";

    // 4. Multi-Factor Authentication (Future)
    public const string MfaEnabledSuccessfully = "Two-factor authentication has been enabled successfully.";
    public const string MfaDisabledSuccessfully = "Two-factor authentication has been disabled.";
    public const string MfaCodeSent = "A two-factor authentication code has been sent to your device.";
}
