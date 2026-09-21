using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;

namespace ECommerce.Domain.Entities.OrderAggregate;

public sealed class ShippingAddress
{
    public const int MaxNameLength = 100;
    public const int MaxPhoneLength = 32;
    public const int MaxCountryLength = 100;
    public const int MaxCityLength = 100;
    public const int MaxStreetLength = 200;
    public const int MaxPostalCodeLength = 20;

    private ShippingAddress() { }

    public string RecipientFirstName { get; private set; } = null!;
    public string RecipientLastName { get; private set; } = null!;
    public string PhoneNumber { get; private set; } = null!;
    public string Country { get; private set; } = null!;
    public string City { get; private set; } = null!;
    public string Street { get; private set; } = null!;
    public string PostalCode { get; private set; } = null!;

    public static Result<ShippingAddress> Create(
        string firstName,
        string lastName,
        string phoneNumber,
        string country,
        string city,
        string street,
        string postalCode)
    {
        var address = new ShippingAddress();

        var firstNameResult = address.SetRecipientFirstName(firstName);
        if (firstNameResult.IsFailure) 
            return Result<ShippingAddress>.Failure(firstNameResult.Error!);

        var lastNameResult = address.SetRecipientLastName(lastName);
        if (lastNameResult.IsFailure) 
            return Result<ShippingAddress>.Failure(lastNameResult.Error!);

        var phoneResult = address.SetPhoneNumber(phoneNumber);
        if (phoneResult.IsFailure) 
            return Result<ShippingAddress>.Failure(phoneResult.Error!);

        var locationResult = address.SetLocation(country, city, street, postalCode);
        if (locationResult.IsFailure) 
            return Result<ShippingAddress>.Failure(locationResult.Error!);

        return Result<ShippingAddress>.Success(address);
    }

    private Result SetRecipientFirstName(string firstName)
    {
        var trimmed = firstName.Trim();
        if (string.IsNullOrWhiteSpace(trimmed)) 
            return Result.Failure(OrderErrors.ShippingAddressFirstNameRequired);

        if (trimmed.Length > MaxNameLength) 
            return Result.Failure(OrderErrors.ShippingAddressFirstNameLengthExceeded);

        RecipientFirstName = trimmed;
        return Result.Success();
    }

    private Result SetRecipientLastName(string lastName)
    {
        var trimmed = lastName.Trim();
        if (string.IsNullOrWhiteSpace(trimmed)) 
            return Result.Failure(OrderErrors.ShippingAddressLastNameRequired);

        if (trimmed.Length > MaxNameLength) 
            return Result.Failure(OrderErrors.ShippingAddressLastNameLengthExceeded);

        RecipientLastName = trimmed;
        return Result.Success();
    }

    private Result SetPhoneNumber(string phoneNumber)
    {
        var trimmed = phoneNumber.Trim();
        if (string.IsNullOrWhiteSpace(trimmed)) 
            return Result.Failure(OrderErrors.ShippingAddressPhoneNumberRequired);

        if (trimmed.Length > MaxPhoneLength) 
            return Result.Failure(OrderErrors.ShippingAddressPhoneNumberLengthExceeded);

        PhoneNumber = trimmed;
        return Result.Success();
    }

    private Result SetLocation(string country, string city, string street, string postalCode)
    {
        var trimmedCountry = country?.Trim();
        var trimmedCity = city?.Trim();
        var trimmedStreet = street?.Trim();
        var trimmedPostalCode = postalCode?.Trim();

        if (string.IsNullOrWhiteSpace(trimmedCountry) || trimmedCountry.Length > MaxCountryLength ||
            string.IsNullOrWhiteSpace(trimmedCity) || trimmedCity.Length > MaxCityLength ||
            string.IsNullOrWhiteSpace(trimmedStreet) || trimmedStreet.Length > MaxStreetLength)
        {
            return Result.Failure(OrderErrors.ShippingAddressInvalidLocation);
        }

        if (string.IsNullOrWhiteSpace(trimmedPostalCode) || trimmedPostalCode.Length > MaxPostalCodeLength)
        {
            return Result.Failure(OrderErrors.ShippingAddressInvalidPostalCode);
        }

        Country = trimmedCountry;
        City = trimmedCity;
        Street = trimmedStreet;
        PostalCode = trimmedPostalCode;

        return Result.Success();
    }

    public static Result<ShippingAddress> FromUserAddress(UserAddress address)
    {
        if (address is null)
            return Result<ShippingAddress>.Failure(OrderErrors.ShippingAddressRequired);

        return Create(
            address.RecipientFirstName,
            address.RecipientLastName,
            address.PhoneNumber,
            address.Country,
            address.City,
            address.Street,
            address.PostalCode);
    }

}
