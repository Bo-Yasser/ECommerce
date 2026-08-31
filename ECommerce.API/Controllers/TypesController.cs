using ECommerce.API.Contracts.Responses;
using ECommerce.UseCases.Features.ProductTypes.Responses;
using ECommerce.UseCases.Features.ProductTypes.Queries.GetTypes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ECommerce.API.Constants;

namespace ECommerce.API.Controllers;

public class TypesController(IMediator mediator) : ApiControllerBase
{
    /// <summary>
    /// Get all types
    /// </summary>
    /// <param name="ct">A CancellationToken used to cancel the request</param>
    /// <returns>
    /// Returns a list of types with their Id and Name
    /// </returns>
    /// <response code="200">Types returned successfully</response>
    [HttpGet] // GET api/types
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<GetTypesResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<GetTypesResponse>>>> GetAll(CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetTypesQuery(), ct);
        if (result.IsFailure)
            return Problem(result);

        return Ok(ApiResponse<IReadOnlyList<GetTypesResponse>>.Ok(
            result.Value,
            HttpContext.TraceIdentifier,
            TypeMessages.ListRetrievedSuccessfully));
    }

}
