namespace ECommerce.Domain.Common.Errors;

using ECommerce.Domain.Entities.OrderAggregate;

public static class OrderErrors
{
    // Order
    public static readonly Error NotFound =
        Error.NotFound("Order.NotFound", "Order not found");

    public static readonly Error InvalidId =
        Error.Validation("Order.InvalidId", "Order Id is invalid");

    public static readonly Error InvalidUserId =
        Error.Validation("Order.InvalidUserId", "User Id is invalid");

    public static readonly Error ShippingAddressRequired =
        Error.Validation("Order.ShippingAddressRequired", "Shipping address is required");

    public static readonly Error EmptyOrderItems =
        Error.Validation("Order.EmptyOrderItems", "An order must contain at least one item");

    public static readonly Error InvalidSubtotal =
         Error.Validation("Order.InvalidSubtotal", "Order subtotal cannot be negative");

    public static readonly Error InvalidStatusTransition =
        Error.Validation("Order.InvalidStatusTransition", "Cannot transition to the requested order status");

    public static readonly Error AlreadyCanceled =
        Error.Conflict("Order.AlreadyCanceled", "Order is already canceled and cannot be modified");

    public static readonly Error AlreadyShipped =
        Error.Conflict("Order.AlreadyShipped", "Order has already been shipped and cannot be canceled or modified");

    public static readonly Error PaymentFailed =
        Error.Failure("Order.PaymentFailed", "Payment process for this order failed");

    public static readonly Error CannotCancel =
        Error.Failure("Order.CannotCancel", "The order cannot be cancelled! only pending orders can be cancelled.");
    public static readonly Error InvalidPaymentState = 
        Error.Conflict("Order.InvalidPaymentState", "Only orders in 'Pending' status can be transitioned to 'Processing'.");

    public static readonly Error OnlyCanEditPendingOrder =
        Error.Conflict("Order.OnlyCanEditPendingOrder", "Only orders in 'Pending' status can be edited.");

    // OrderItem
    public static readonly Error OrderItemInvalidId =
        Error.Validation("Order.OrderItem.InvalidId", "Order Item Id is invalid");

    public static readonly Error OrderItemNotFound =
        Error.NotFound("Order.OrderItemNotFound", "Order item not found");

    public static readonly Error OrderItemProductRequired =
        Error.Validation("Order.OrderItem.ProductRequired", "Product item details are required for the order item");

    public static readonly Error OrderItemInvalidQuantity =
         Error.Validation("Order.OrderItem.InvalidQuantity", "Order item quantity must be greater than zero");

    // ProductItemOrdered
    public static readonly Error ProductItemInvalidId =
        Error.Validation("Order.ProductItem.InvalidId", "Product ID in order item is invalid");

    public static readonly Error ProductItemNameRequired =
        Error.Validation("Order.ProductItem.NameRequired", "Product name in order item is required");

    public static readonly Error ProductItemNameLengthExceeded =
        Error.Validation("Order.ProductItem.NameLengthExceeded", $"Product name in order item cannot exceed {ProductItemOrdered.MaxProductNameLength} characters.");

    public static readonly Error ProductItemPictureUrlRequired =
        Error.Validation("Order.ProductItem.PictureUrlRequired", "Product picture URL in order item is required");

    public static readonly Error ProductItemPictureUrlLengthExceeded =
        Error.Validation("Order.ProductItem.PictureUrlLengthExceeded", $"Product picture URL in order item cannot exceed {ProductItemOrdered.MaxPictureUrlLength} characters.");

    public static readonly Error ProductItemNegativePrice =
         Error.Validation("Order.ProductItem.NegativePrice", "Product unit price in order item cannot be negative.");

    // OrderDeliveryMethod
    public static readonly Error OrderDeliveryMethodRequired =
        Error.Validation("Order.DeliveryMethod.Required", "Delivery Method is required");

    public static readonly Error OrderDeliveryMethodUnavailable =
        Error.Validation("Order.DeliveryMethod.Unavailable", "Delivery Method is unavailable");


    public static readonly Error OrderDeliveryMethodInvalidId =
        Error.Validation("Order.DeliveryMethod.InvalidId", "Delivery Method Id is invalid");

    public static readonly Error OrderDeliveryMethodNameRequired =
        Error.Validation("Order.DeliveryMethod.NameRequired", "Delivery method name is required");

    public static readonly Error OrderDeliveryMethodNameLengthExceeded =
        Error.Validation("Order.DeliveryMethod.NameLengthExceeded", $"Delivery method name cannot exceed {OrderDeliveryMethod.MaxNameLength} characters.");

    public static readonly Error OrderDeliveryMethodTimeRequired =
        Error.Validation("Order.DeliveryMethod.TimeRequired", "Delivery method estimated time is required");

    public static readonly Error OrderDeliveryMethodTimeLengthExceeded =
        Error.Validation("Order.DeliveryMethod.TimeLengthExceeded", $"Delivery method estimated time cannot exceed {OrderDeliveryMethod.MaxEstimatedTimeLength} characters.");

    public static readonly Error OrderDeliveryMethodNegativePrice =
         Error.Validation("Order.DeliveryMethod.NegativePrice", "Delivery method price cannot be negative.");

    // ShippingAddress
    public static readonly Error ShippingAddressFirstNameRequired =
        Error.Validation("Order.ShippingAddress.FirstNameRequired", "Recipient first name is required");
    public static readonly Error ShippingAddressFirstNameLengthExceeded =
        Error.Validation("Order.ShippingAddress.FirstNameLengthExceeded", $"First name cannot exceed {ShippingAddress.MaxNameLength} characters.");

    public static readonly Error ShippingAddressLastNameRequired =
        Error.Validation("Order.ShippingAddress.LastNameRequired", "Recipient last name is required");
    public static readonly Error ShippingAddressLastNameLengthExceeded =
        Error.Validation("Order.ShippingAddress.LastNameLengthExceeded", $"Last name cannot exceed {ShippingAddress.MaxNameLength} characters.");

    public static readonly Error ShippingAddressPhoneNumberRequired =
        Error.Validation("Order.ShippingAddress.PhoneNumberRequired", "Phone number is required");
    public static readonly Error ShippingAddressPhoneNumberLengthExceeded =
        Error.Validation("Order.ShippingAddress.PhoneNumberLengthExceeded", $"Phone number cannot exceed {ShippingAddress.MaxPhoneLength} characters.");

    public static readonly Error ShippingAddressInvalidLocation =
            Error.Validation("Order.ShippingAddress.InvalidLocation", "Country, City, and Street are required and must be within valid length limits.");

    public static readonly Error ShippingAddressInvalidPostalCode =
        Error.Validation("Order.ShippingAddress.InvalidPostalCode", "Postal code is required and must be within valid length limits.");

    public static readonly Error ShippingAddressNotOwned =
    Error.Validation("Order.ShippingAddress.NotOwned", "The shipping address does not belong to this user.");
}