using ECommerce.UseCases.ProductBrands.Dtos;
using ECommerce.UseCases.ProductBrands.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

public class BrandsController(
    GetAllBrandsQuery getAllBrandsQuery) : ApiControllerBase
{
    [HttpGet] // GET api/brands
    public async Task<ActionResult<IReadOnlyList<GetAllBrandsResponse>>> GetAll(CancellationToken ct = default)
    public async Task<ActionResult<ApiResponse<IReadOnlyList<GetAllBrandsResponse>>>> GetAll(CancellationToken ct = default)
    {
        var result = await getAllBrandsQuery.ExecuteAsync(ct);
        if (result.IsFailure)
            return Problem(result);

        return Ok(ApiResponse<IReadOnlyList<GetAllBrandsResponse>>.Ok(result.Value, HttpContext.TraceIdentifier));
    }
}
