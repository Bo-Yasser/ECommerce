namespace ECommerce.Domain.Common.Errors;

// Result<Product>.Failure(Error)
// Result<Product>.Failure("Product.NotFound", "Product Not Found", ErrorType.NotFound)
// Code: improtant for logs and filtering
public sealed record Error(string Code, string Message, ErrorType Type)
{
    public static Error Validation(string code, string message)
        => new(code, message, ErrorType.Validation);
    public static Error NotFound(string code, string message)
        => new(code, message, ErrorType.NotFound);
    public static Error Conflict(string code, string message)
        => new(code, message, ErrorType.Conflict);
    public static Error UnAuthorized(string code, string message)
        => new(code, message, ErrorType.UnAuthorized);

    public static Error Forbidden(string code, string message)
        => new(code, message, ErrorType.Forbidden);
    public static Error Failure(string code, string message)
        => new(code, message, ErrorType.Failure);

}