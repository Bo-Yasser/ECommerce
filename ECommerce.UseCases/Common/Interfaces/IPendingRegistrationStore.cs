using ECommerce.UseCases.Common.Models;

namespace ECommerce.UseCases.Common.Interfaces;

public interface IPendingRegistrationStore
{
    Task SaveAsync(string email, PendingRegistrationPayload payload, CancellationToken ct = default);
    Task<PendingRegistrationPayload?> ValidateAndConsumeAsync(string email, string code, CancellationToken ct = default);
    Task RemoveAsync(string email, CancellationToken ct = default);
    Task<PendingRegistrationPayload?> GetAsync(string email, CancellationToken ct = default);
}