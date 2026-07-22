namespace ECommerce.Domain.Common.Errors;

public enum ErrorType
{
    Validation = 1,   // 400 Bad Request (uncomplete data, wrong format, ..)
    NotFound = 2,     // 404 
    Conflict = 3,     // 409
    UnAuthorized = 4, // 401
    Forbidden = 5,    // 403
    Failure = 6       // 500
}
