using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Common.Interfaces;
using ECommerce.UseCases.Features.Users.Responses;
using ECommerce.UseCases.Features.Users.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.Users.Commands.AddUserAddress;

public sealed class AddUserAddressCommandHandler(
    ICurrentUserService currentUser,
    IRepository<UserAddress> addressRepository,
    IReadRepository<UserAddress> addressReadRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<AddUserAddressCommand, Result<UserAddressResponse>>
{

    public async Task<Result<UserAddressResponse>> Handle(AddUserAddressCommand request, CancellationToken cancellationToken)
    {
        if (currentUser.UserId is null)
            return Result<UserAddressResponse>.Failure(AuthErrors.InvalidCredentials);

        var createResult = UserAddress.Create(
            Guid.NewGuid(),
            currentUser.UserId.Value,
            request.RecipientFirstName,
            request.RecipientLastName,
            request.PhoneNumber,
            request.Country,
            request.City,
            request.Street,
            request.PostalCode,
            request.IsDefault);

        if (createResult.IsFailure)
            return Result<UserAddressResponse>.Failure(createResult.Error!);

        if(request.IsDefault)
            await RevokeOldDefaultAddressAsync(currentUser.UserId.Value, cancellationToken);

        var address = createResult.Value;
        addressRepository.Add(address);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<UserAddressResponse>.Success(new UserAddressResponse(
            address.Id,
            address.RecipientFirstName,
            address.RecipientLastName,
            address.PhoneNumber,
            address.Country,
            address.City,
            address.Street,
            address.PostalCode,
            address.IsDefault));
    }

    private async Task RevokeOldDefaultAddressAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var defaultAddresses = await addressReadRepository.ListAsync(
            new DefaultUserAddressesByIdSpecification(userId),
            cancellationToken);

        if (defaultAddresses is null || defaultAddresses.Count == 0) return;

        foreach(var defaultAddress in defaultAddresses)
        {
            defaultAddress.RemoveDefaultStatus();
            addressRepository.Update(defaultAddress);
        }
    }
}
