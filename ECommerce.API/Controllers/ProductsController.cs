using ECommerce.UseCases.Products.Dtos;
using ECommerce.UseCases.Products.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

public class ProductsController(
    GetAllProductsQuery getAllProductsQuery,
    GetByIdProductQuery getByIdProductQuery) : ApiControllerBase
{
    [HttpGet] // GET api/products
    public async Task<ActionResult<IReadOnlyList<GetAllProductsResponse>>> GetAll(CancellationToken ct = default)
    public async Task<ActionResult<ApiResponse<IReadOnlyList<GetAllProductsResponse>>>> GetAll(CancellationToken ct = default)
    {
        var result = await getAllProductsQuery.ExecuteAsync(ct);
        if (result.IsFailure)
            return Problem(result);
        return Ok(ApiResponse<IReadOnlyList<GetAllProductsResponse>>.Ok(result.Value, HttpContext.TraceIdentifier));
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetByIdProductResponse>> GetById(Guid id, CancellationToken ct = default)
    public async Task<ActionResult<ApiResponse<GetByIdProductResponse>>> GetById(Guid id, CancellationToken ct = default)
    {
        var result = await getByIdProductQuery.ExecuteAsync(id, ct);
        if (result.IsFailure)
            return Problem(result);

        return Ok(ApiResponse<GetByIdProductResponse>.Ok(result.Value, HttpContext.TraceIdentifier));
    }
}