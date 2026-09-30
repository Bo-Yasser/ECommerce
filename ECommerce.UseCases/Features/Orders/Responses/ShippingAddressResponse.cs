using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.OrderAggregate;

namespace ECommerce.UseCases.Features.Orders.Responses;

public sealed record ShippingAddressResponse(
    string RecipientFirstName,
    string RecipientLastName,
    string PhoneNumber,
    string Country,
    string City,
    string Street,
    string PostalCode)
{
    public static ShippingAddressResponse From(ShippingAddress shippingAddress)
        => new(
            shippingAddress.RecipientFirstName,
            shippingAddress.RecipientLastName,
            shippingAddress.PhoneNumber,
            shippingAddress.Country,
            shippingAddress.City,
            shippingAddress.Street,
            shippingAddress.PostalCode);
}
