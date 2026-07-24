using ECommerce.UseCases.ProductBrands.Dtos;
using ECommerce.UseCases.ProductBrands.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

public class BrandsController(
    GetAllBrandsQuery getAllBrandsQuery) : ApiControllerBase
{
    [HttpGet] // GET api/brands
    public async Task<ActionResult<IReadOnlyList<GetAllBrandsResponse>>> GetAll(CancellationToken ct = default)
    {
        var result = await getAllBrandsQuery.ExecuteAsync(ct);
        return Ok(result.Value);
    }
}
