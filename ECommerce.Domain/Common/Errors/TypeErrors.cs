namespace ECommerce.Domain.Common.Errors;

public static class TypeErrors
{
    public const int MaxNameLength = 100;

    public static readonly Error InvalidId = 
        Error.Validation("Type.InvalidId", "Type id is invalid");

    public static readonly Error NameRequired = 
        Error.Validation("Type.NameRequired", "Type name is required");

    public static readonly Error NameLengthExceeded = 
        Error.Validation("Type.NameLengthExceeded", $"Type Name Cannot Exceed {MaxNameLength} characters.");
}
