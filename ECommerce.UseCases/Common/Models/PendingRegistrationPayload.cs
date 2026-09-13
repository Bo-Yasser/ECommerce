namespace ECommerce.UseCases.Common.Models;

public sealed record PendingRegistrationPayload(
    string FirstName,
    string LastName,
    string Email,
    string PasswordHash,
    string VerificationCode,
    int FailedAttempts,
    DateTimeOffset LastOtpSendTime);
