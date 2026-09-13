namespace ECommerce.Domain.Common.Errors;

public static class UserAddressErrors
{
    public static readonly Error NotFound =
        Error.NotFound(
            "UserAddress.NotFound",
            "Address was not found.");

    public static readonly Error InvalidId =
        Error.Validation(
            "UserAddress.InvalidId",
            "Address Id is required.");

    public static readonly Error InvalidUserId =
        Error.Validation(
            "UserAddress.InvalidUserId",
            "User Id is required.");

    public static readonly Error InvalidName =
        Error.Validation(
            "UserAddress.InvalidName",
            "Recipient name is required.");

    public static readonly Error InvalidPhoneNumber =
        Error.Validation(
            "UserAddress.InvalidPhone",
            "Phone number is required.");

    public static readonly Error InvalidLocation =
        Error.Validation(
            "UserAddress.InvalidLocation",
            "Country, City and Street are required.");

    public static readonly Error InvalidPostalCode =
        Error.Validation(
            "UserAddress.InvalidPostalCode",
            "Postal code is required.");

}
