using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;

namespace ECommerce.Domain.Entities;

public class UserAddress : BaseEntity
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
        if (string.IsNullOrWhiteSpace(firstName) || firstName.Length > MaxNameLength ||
            string.IsNullOrWhiteSpace(lastName) || lastName.Length > MaxNameLength)
        {
            return Result.Failure(UserAddressErrors.InvalidName);
        }
        RecipientFirstName = firstName;
        RecipientLastName = lastName;
        return Result.Success();
    }
    private Result SetPhoneNumber(string phoneNumber)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber) || phoneNumber.Length > MaxPhoneLength)
        {
            return Result.Failure(UserAddressErrors.InvalidPhoneNumber);
        }
        PhoneNumber = phoneNumber;
        return Result.Success();
    }
    private Result SetLocation(string country, string city, string street, string postalCode)
    {
        if (string.IsNullOrWhiteSpace(country) || country.Length > MaxCountryLength ||
            string.IsNullOrWhiteSpace(city) || city.Length > MaxCityLength ||
            string.IsNullOrWhiteSpace(street) || street.Length > MaxStreetLength)
        {
            return Result.Failure(UserAddressErrors.InvalidLocation);
        }
        if (string.IsNullOrWhiteSpace(postalCode) || postalCode.Length > MaxPostalCodeLength)
        {
            return Result.Failure(UserAddressErrors.InvalidPostalCode);
        }

        Country = country;
        City = city;
        Street = street;
        PostalCode = postalCode;

        return Result.Success();
    }
    public void RemoveDefaultStatus() => IsDefault = false;
}