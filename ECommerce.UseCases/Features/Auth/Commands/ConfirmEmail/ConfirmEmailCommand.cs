using ECommerce.Domain.Common;
using MediatR;

namespace ECommerce.UseCases.Features.Auth.Commands.ConfirmEmail;

public sealed record ConfirmEmailCommand(
    string Email,
    string VerificationCode) : IRequest<Result>;