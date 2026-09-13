using System.Text.Json.Serialization;

namespace ECommerce.API.Contracts.Responses;

public class ApiResponse
{
    public bool Success { get; set; } = true;
    public string? Message { get; set; }
    public ApiMeta Meta { get; set; } = new();

    public static ApiResponse Ok(string traceId, string? message = null)
    {
        return new ApiResponse
        {
            Success = true,
            Message = message,
            Meta = new ApiMeta { TraceId = traceId }
        };
    }
}

public sealed class ApiResponse<T> : ApiResponse
{
    public T? Data { get; set; }

    public static ApiResponse<T> Ok(
        T data,
        string traceId,
        string? message = null,
        PaginationMeta? pagination = null)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = data,
            Message = message,
            Meta = new ApiMeta
            {
                TraceId = traceId,
                Pagination = pagination
            }
        };
    }
}

public sealed class ApiMeta
{
    public string TraceId { get; set; } = null!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public PaginationMeta? Pagination { get; set; }
}

public sealed class PaginationMeta(int pageNumber, int pageSize, int totalCount)
{
    public int PageNumber { get; init; } = pageNumber;
    public int PageSize { get; init; } = pageSize;
    public int TotalCount { get; init; } = totalCount;

    public int TotalPages => (int)Math.Ceiling(totalCount / (double)pageSize);
    public bool HasPreviousPage => pageNumber > 1;
    public bool HasNextPage => pageNumber < TotalPages;
}