using ECommerce.Domain.Common;
using ECommerce.UseCases.Features.Users.Responses;
using MediatR;

namespace ECommerce.UseCases.Features.Users.Commands.AddUserAddress;

public sealed record AddUserAddressCommand(
    string RecipientFirstName,
    string RecipientLastName,
    string PhoneNumber,
    string Country,
    string City,
    string Street,
    string PostalCode,
    bool IsDefault = false) : IRequest<Result<UserAddressResponse>>;
