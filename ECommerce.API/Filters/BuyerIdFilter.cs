using ECommerce.API.Controllers;
using ECommerce.API.Extensions;
using ECommerce.Domain.Common.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ECommerce.API.Filters;

public class BuyerIdFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var controller = context.Controller as ControllerBase;


        if (controller == null)
        {
            await next();
            return;
        }

        var buyerIdResult = controller.GetBuyerId();

        if (buyerIdResult.IsFailure)
        {
            var statusCode = buyerIdResult.Error!.Type switch
            {
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.UnAuthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError
            };

            var title = buyerIdResult.Error.Type switch
            {
                ErrorType.Validation => "Validation Error",
                ErrorType.NotFound => "Resource Not Found",
                ErrorType.UnAuthorized => "Unauthorized Access",
                ErrorType.Forbidden => "Forbidden Access",
                _ => "Internal Server Error"
            };

            var problem = new Dictionary<string, object?>
            {
                ["type"] = $"https://example.com/errors/{buyerIdResult.Error.Code}",
                ["title"] = title,
                ["status"] = statusCode
            };

            if (buyerIdResult.Error.Type is ErrorType.Validation)
            {
                problem["errors"] = new Dictionary<string, string[]>
                {
                    [buyerIdResult.Error.Code] = [buyerIdResult.Error.Message]
                };
            }
            else
            {
                problem["details"] = buyerIdResult.Error.Message;
            }
            problem["traceId"] = context.HttpContext.TraceIdentifier;

            context.Result = new ObjectResult(problem) { StatusCode = statusCode };
            return;
        }

        context.HttpContext.Items["ValidatedBuyerId"] = buyerIdResult.Value;

        await next();
    }
}