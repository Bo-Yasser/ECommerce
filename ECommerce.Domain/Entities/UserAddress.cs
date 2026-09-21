using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;

namespace ECommerce.Domain.Entities;

public sealed class UserAddress : BaseEntity
{
    public const int MaxNameLength = 100;
    public const int MaxPhoneLength = 32;
    public const int MaxCountryLength = 100;
    public const int MaxCityLength = 100;
    public const int MaxStreetLength = 200;
    public const int MaxPostalCodeLength = 20;

    private UserAddress()
    {
    }

    public Guid UserId { get; private set; }
    public string RecipientFirstName { get; private set; } = null!;
    public string RecipientLastName { get; private set; } = null!;
    public string PhoneNumber { get; private set; } = null!;
    public string Country { get; private set; } = null!;
    public string City { get; private set; } = null!;
    public string Street { get; private set; } = null!;
    public string PostalCode { get; private set; } = null!;
    public bool IsDefault { get; private set; }

    public static Result<UserAddress> Create(
        Guid id,
        Guid userId,
        string recipientFirstName,
        string recipientLastName,
        string phoneNumber,
        string country,
        string city,
        string street,
        string postalCode,
        bool isDefault = false)
    {
        var userAddress = new UserAddress();

        var idAndUserIdResult = userAddress.SetIdAndUserId(userId, id);
        if (idAndUserIdResult.IsFailure)
            return Result<UserAddress>.Failure(idAndUserIdResult.Error!);

        var recipientNameResult = userAddress.SetRecipientName(recipientFirstName, recipientLastName);
        if (recipientNameResult.IsFailure)
            return Result<UserAddress>.Failure(recipientNameResult.Error!);

        var phoneNumberResult = userAddress.SetPhoneNumber(phoneNumber);
        if (phoneNumberResult.IsFailure)
            return Result<UserAddress>.Failure(phoneNumberResult.Error!);

        var locationResult = userAddress.SetLocation(country, city, street, postalCode);
        if (locationResult.IsFailure)
            return Result<UserAddress>.Failure(locationResult.Error!);

        userAddress.IsDefault = isDefault;

        return Result<UserAddress>.Success(userAddress);
    }
    public Result Update(
        string recipientFirstName,
        string recipientLastName,
        string phoneNumber,
        string country,
        string city,
        string street,
        string postalCode,
        bool isDefault)
    {
        var recipientNameResult = SetRecipientName(recipientFirstName, recipientLastName);
        if (recipientNameResult.IsFailure)
            return Result.Failure(recipientNameResult.Error!);

        var phoneNumberResult = SetPhoneNumber(phoneNumber);
        if (phoneNumberResult.IsFailure)
            return Result.Failure(phoneNumberResult.Error!);

        var locationResult = SetLocation(country, city, street, postalCode);
        if (locationResult.IsFailure)
            return Result.Failure(locationResult.Error!);

        IsDefault = isDefault;
        return Result.Success();
    }


    private Result SetIdAndUserId(Guid userId, Guid id)
    {
        if (id == Guid.Empty)
        {
            return Result.Failure(UserAddressErrors.InvalidId);
        }

        if (userId == Guid.Empty)
        {
            return Result.Failure(UserAddressErrors.InvalidUserId);
        }

        Id = id;
        UserId = userId;
        return Result.Success();
    }
    private Result SetRecipientName(string firstName, string lastName)
    {
        var trimmedFirstName = firstName.Trim();
        var trimmedLastName = lastName.Trim();
        if (string.IsNullOrWhiteSpace(trimmedFirstName) || trimmedFirstName.Length > MaxNameLength ||
            string.IsNullOrWhiteSpace(trimmedLastName) || trimmedLastName.Length > MaxNameLength)
        {
            return Result.Failure(UserAddressErrors.InvalidName);
        }
        RecipientFirstName = trimmedFirstName;
        RecipientLastName = trimmedLastName;
        return Result.Success();
    }
    private Result SetPhoneNumber(string phoneNumber)
    {
        var trimmedPhoneNumber = phoneNumber.Trim();
        if (string.IsNullOrWhiteSpace(trimmedPhoneNumber) || trimmedPhoneNumber.Length > MaxPhoneLength)
        {
            return Result.Failure(UserAddressErrors.InvalidPhoneNumber);
        }
        PhoneNumber = trimmedPhoneNumber;
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
            return Result.Failure(UserAddressErrors.InvalidLocation);
        }

        if (string.IsNullOrWhiteSpace(trimmedPostalCode) || trimmedPostalCode.Length > MaxPostalCodeLength)
        {
            return Result.Failure(UserAddressErrors.InvalidPostalCode);
        }

        Country = trimmedCountry;
        City = trimmedCity;
        Street = trimmedStreet;
        PostalCode = trimmedPostalCode;

        return Result.Success();
    }
    public void RemoveDefaultStatus() => IsDefault = false;
}