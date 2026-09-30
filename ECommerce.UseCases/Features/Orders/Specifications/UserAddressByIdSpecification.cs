using ECommerce.Domain.Entities;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Features.Orders.Specifications;

public sealed class UserAddressByIdSpecification : Specification<UserAddress>
{
    public UserAddressByIdSpecification(Guid addressId, Guid userId)
        => Query.Where(a => a.Id == addressId && a.UserId == userId);
}