using ECommerce.Domain.Common;
using ECommerce.UseCases.Features.Users.Responses;
using MediatR;

namespace ECommerce.UseCases.Features.Users.Queries.GetCurrentUser;

public sealed record GetCurrentUserQuery : IRequest<Result<UserProfileResponse>>;
