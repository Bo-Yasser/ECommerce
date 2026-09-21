using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Common.Errors;

public static class TypeErrors
{
    public static readonly Error NotFound =
        Error.NotFound("Type.NotFound", "Type Not Found");

    public static readonly Error InvalidId = 
        Error.Validation("Type.InvalidId", "Type id is invalid");

    public static readonly Error NameRequired = 
        Error.Validation("Type.NameRequired", "Type name is required");

    public static readonly Error NameLengthExceeded = 
        Error.Validation("Type.NameLengthExceeded", $"Type Name Cannot Exceed {ProductType.MaxNameLength} characters.");

    public static readonly Error AlreadyExists =
        Error.Conflict("Type.AlreadyExists", "Type already exists");
}
