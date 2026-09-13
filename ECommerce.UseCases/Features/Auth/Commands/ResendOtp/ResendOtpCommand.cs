using ECommerce.Domain.Common;
using MediatR;

namespace ECommerce.UseCases.Features.Auth.Commands.ResendOtp;

public sealed record ResendOtpCommand(string Email) : IRequest<Result>;