using ECommerce.API.Models;
using ECommerce.UseCases.ProductTypes.Dtos;
using ECommerce.UseCases.ProductTypes.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

public class TypesController(GetAllTypesQuery getAllTypesQuery) : ApiControllerBase
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
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<GetAllTypesResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<GetAllTypesResponse>>>> GetAll(CancellationToken ct = default)
    {
        var result = await getAllTypesQuery.ExecuteAsync(ct);
        if (result.IsFailure)
            return Problem(result);

        return Ok(ApiResponse<IReadOnlyList<GetAllTypesResponse>>.Ok(result.Value, HttpContext.TraceIdentifier));
    }

}
