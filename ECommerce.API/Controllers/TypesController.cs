using ECommerce.UseCases.ProductTypes.Dtos;
using ECommerce.UseCases.ProductTypes.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

public class TypesController(GetAllTypesQuery getAllTypesQuery) : ApiControllerBase
{
    [HttpGet] // GET api/types
    public async Task<ActionResult<IReadOnlyList<GetAllTypesResponse>>> GetAll(CancellationToken ct = default)
    {
        var result = await getAllTypesQuery.ExecuteAsync(ct);
        return Ok(result.Value);
    }

}
