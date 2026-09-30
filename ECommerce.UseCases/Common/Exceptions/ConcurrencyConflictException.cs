namespace ECommerce.UseCases.Common.Exceptions;
public sealed class ConcurrencyConflictException : Exception
{
    public ConcurrencyConflictException(string? message = null, Exception? innerException = null)
        : base(message, innerException)
    {
    }
}
