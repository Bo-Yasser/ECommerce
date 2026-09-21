namespace ECommerce.Domain.Common.Errors;

using ECommerce.Domain.Entities;

public static class DeliveryMethodErrors
{
    public static readonly Error NotFound =
        Error.NotFound("DeliveryMethod.NotFound", "Delivery method not found");

    public static readonly Error NoAvailableDeliveryMethods =
        Error.NotFound("DeliveryMethod.NoAvailableDeliveryMethods", "No delivery methods are available.");

    public static readonly Error InvalidId =
        Error.Validation("DeliveryMethod.InvalidId", "Delivery method Id is invalid");

    public static readonly Error NameRequired =
        Error.Validation("DeliveryMethod.NameRequired", "Delivery method name is required");

    public static readonly Error EstimatedDeliveryTimeRequired =
        Error.Validation("DeliveryMethod.EstimatedDeliveryTimeRequired", "Estimated delivery time is required");

    public static readonly Error NameLengthExceeded =
        Error.Validation("DeliveryMethod.NameLengthExceeded", $"Delivery method name cannot exceed {DeliveryMethod.MaxNameLength} characters.");

    public static readonly Error DescriptionLengthExceeded =
        Error.Validation("DeliveryMethod.DescriptionLengthExceeded", $"Delivery method description cannot exceed {DeliveryMethod.MaxDescriptionLength} characters.");

    public static readonly Error EstimatedDeliveryTimeLengthExceeded =
        Error.Validation("DeliveryMethod.EstimatedDeliveryTimeLengthExceeded", $"Estimated delivery time cannot exceed {DeliveryMethod.MaxDeliveryTimeLength} characters.");

    public static readonly Error NegativePrice =
         Error.Validation("DeliveryMethod.NegativePrice", "Delivery method price cannot be negative.");

    public static readonly Error AlreadyExists =
        Error.Conflict("DeliveryMethod.AlreadyExists", "Delivery method already exists");

    public static readonly Error NameAlreadyExists =
    Error.Conflict("DeliveryMethod.NameAlreadyExists", "Delivery method name already exists");

    public static readonly Error CreateFailed =
        Error.Failure("DeliveryMethod.CreateFailed", "Delivery method could not be created");

    public static readonly Error UpdateFailed =
        Error.Failure("DeliveryMethod.UpdateFailed", "Delivery method could not be updated");

    public static readonly Error DeleteFailed =
        Error.Failure("DeliveryMethod.DeleteFailed", "Delivery method could not be deleted");
}