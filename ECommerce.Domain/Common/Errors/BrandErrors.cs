using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Common.Errors;

public static class BrandErrors
{
    public static readonly Error NotFound =
        Error.NotFound("Brand.NotFound", "Brand Not Found");

    public static readonly Error InvalidId =
        Error.Validation("Brand.InvalidId", "Brand id is invalid");

    public static readonly Error NameRequired =
        Error.Validation("Brand.NameRequired", "Brand name is required");

    public static readonly Error NameLengthExceeded =
        Error.Validation("Brand.NameLengthExceeded", $"Brand Name Cannot Exceed {ProductBrand.MaxNameLength} characters.");

    public static readonly Error AlreadyExists =
        Error.Conflict("Brand.AlreadyExists", "Brand already exists");
}
