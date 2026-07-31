using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApiControllerBase : ControllerBase
{

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
}
