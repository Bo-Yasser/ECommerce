using ECommerce.UseCases.ProductTypes.Dtos;
using ECommerce.UseCases.ProductTypes.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

public class TypesController(GetAllTypesQuery getAllTypesQuery) : ApiControllerBase
{
    [HttpGet] // GET api/types
    public async Task<ActionResult<IReadOnlyList<GetAllTypesResponse>>> GetAll(CancellationToken ct = default)
    public async Task<ActionResult<ApiResponse<IReadOnlyList<GetAllTypesResponse>>>> GetAll(CancellationToken ct = default)
    {
        var result = await getAllTypesQuery.ExecuteAsync(ct);
        if (result.IsFailure)
            return Problem(result);

        return Ok(ApiResponse<IReadOnlyList<GetAllTypesResponse>>.Ok(result.Value, HttpContext.TraceIdentifier));
    }

}
