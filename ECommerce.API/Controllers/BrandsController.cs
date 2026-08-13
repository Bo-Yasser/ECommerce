using ECommerce.API.Contracts.Responses;
using ECommerce.UseCases.ProductBrands.Queries.GetBrands;
using ECommerce.UseCases.ProductBrands.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

public class BrandsController(IMediator mediator) : ApiControllerBase
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
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<GetBrandsResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<GetBrandsResponse>>>> GetAll(CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetBrandsQuery(), ct);
        if (result.IsFailure)
            return Problem(result);

        return Ok(ApiResponse<IReadOnlyList<GetBrandsResponse>>.Ok(result.Value, HttpContext.TraceIdentifier));
    }
}
