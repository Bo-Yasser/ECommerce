using Asp.Versioning;
using ECommerce.API.Contracts.Responses;
using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.UseCases.Common.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class ApiControllerBase : ControllerBase
{
    protected ActionResult<ApiResponse<T>> Success<T>(
        T data,
        string message,
        PaginationMeta? pagination = null)
        => Ok(ApiResponse<T>.Ok(data, HttpContext.TraceIdentifier, message, pagination));

    protected ActionResult Problem(Result result)
    {
        var statusCode = result.Error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.UnAuthorized => StatusCodes.Status401Unauthorized,
            ErrorType.Forbidden => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError
        };

        var title = result.Error.Type switch
        {
            ErrorType.Validation => "Validation Error",
            ErrorType.NotFound => "Resoruce Not Found",
            ErrorType.UnAuthorized => "Unauthorized Access",
            ErrorType.Forbidden => "Forbidden Access",
            _ => "Internal Server Error"
        };

        var problem = new Dictionary<string, object?>
        {
            ["type"]= $"https://example.com/errors/{result.Error.Code}",
            ["title"]= title,
            ["status"]= statusCode
        };

        if(result.Error.Type is ErrorType.Validation)
        {
            problem["errors"] = new Dictionary<string, string[]>
            {
                [result.Error.Code] = [result.Error.Message]
            };
        }
        else
        {
            problem["details"] = result.Error.Message;
        }
        problem["traceId"] = HttpContext.TraceIdentifier;

        return new ObjectResult(problem)
        {
            StatusCode = statusCode
        };
    }

    protected ActionResult<ApiResponse<IReadOnlyList<T>>> FromPagedResult<T>(
        Result<PagedResult<T>> result,
        int pageNumber,
        int pageSize,
        string successMessage)
    {
        return result.IsFailure 
            ? Problem(result)
            : Success(
                result.Value.Items,
                successMessage,
                new PaginationMeta(pageNumber, pageSize, result.Value.TotalCount));
    }
}
