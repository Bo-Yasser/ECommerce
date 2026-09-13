using ECommerce.Domain.Common;
using ECommerce.UseCases.Features.Users.Responses;
using MediatR;

namespace ECommerce.UseCases.Features.Users.Queries.GetUserAddresses;

public sealed record GetUserAddressesQuery : IRequest<Result<IReadOnlyList<UserAddressResponse>>>;
