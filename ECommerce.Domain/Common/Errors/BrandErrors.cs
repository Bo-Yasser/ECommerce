namespace ECommerce.Domain.Common.Errors;

public static class BrandErrors
{
    public const int MaxNameLength = 100;

    public static readonly Error InvalidId =
        Error.Validation("Brand.InvalidId", "Brand id is invalid");

    public static readonly Error NameRequired =
        Error.Validation("Brand.NameRequired", "Brand name is required");

    public static readonly Error NameLengthExceeded =
        Error.Validation("Brand.NameLengthExceeded", $"Brand Name Cannot Exceed {MaxNameLength} characters.");
}
