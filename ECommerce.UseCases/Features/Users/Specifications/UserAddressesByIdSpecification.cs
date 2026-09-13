using ECommerce.Domain.Entities;
using ECommerce.UseCases.Features.Users.Responses;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.Users.Specifications;

public sealed class UserAddressesByIdSpecification : Specification<UserAddress, UserAddressResponse>
{
    public UserAddressesByIdSpecification(Guid userId)
    {
        Query 
            .Where(address => address.UserId == userId)
            .Select(address => new UserAddressResponse(
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
}
