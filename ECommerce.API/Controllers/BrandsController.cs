using ECommerce.API.Models;
using ECommerce.UseCases.ProductBrands.Dtos;
using ECommerce.UseCases.ProductBrands.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

public class BrandsController(
    GetAllBrandsQuery getAllBrandsQuery) : ApiControllerBase
{
    /// <summary>
    /// Get all brands
    /// </summary>
    /// <param name="ct">A CancellationToken used to cancel the request</param>
    /// <returns>
    /// Returns a list of brands with their Id and Name
    /// </returns>
    /// <response code="200">Brands returned successfully</response>
    [HttpGet] // GET api/brands
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<GetAllBrandsResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<GetAllBrandsResponse>>>> GetAll(CancellationToken ct = default)
    {
        var result = await getAllBrandsQuery.ExecuteAsync(ct);
        if (result.IsFailure)
            return Problem(result);

        return Ok(ApiResponse<IReadOnlyList<GetAllBrandsResponse>>.Ok(result.Value, HttpContext.TraceIdentifier));
    }
}
