using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Common.Interfaces;
using ECommerce.UseCases.Features.Users.Responses;
using ECommerce.UseCases.Features.Users.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.Users.Queries.GetUserAddresses;

public sealed class GetUserAddressesQueryHandler(
    ICurrentUserService currentUser,
    IReadRepository<UserAddress> addressRepository) 
    : IRequestHandler<GetUserAddressesQuery, Result<IReadOnlyList<UserAddressResponse>>>
{
    public async Task<Result<IReadOnlyList<UserAddressResponse>>> Handle(
        GetUserAddressesQuery request,
        CancellationToken cancellationToken)
    {
        if (currentUser.UserId is null)
            return Result<IReadOnlyList<UserAddressResponse>>.Failure(AuthErrors.InvalidCredentials);

        var addresses = await addressRepository.ListAsync(
            new UserAddressesByIdSpecification(currentUser.UserId.Value),
            cancellationToken);

        return Result<IReadOnlyList<UserAddressResponse>>.Success(addresses);
    }
}
