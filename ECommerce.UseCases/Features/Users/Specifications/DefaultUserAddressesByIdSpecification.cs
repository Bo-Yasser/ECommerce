using ECommerce.Domain.Entities;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.Users.Specifications;

public sealed class DefaultUserAddressesByIdSpecification : Specification<UserAddress>
{
    public DefaultUserAddressesByIdSpecification(Guid userId)
    {
        Query
            .Where(x => x.UserId == userId && x.IsDefault == true)
            .AsTracking();
    }
}