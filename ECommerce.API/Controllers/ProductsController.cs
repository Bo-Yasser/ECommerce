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
    {
        var result = await getAllProductsQuery.ExecuteAsync(ct);
        return Ok(result.Value);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetByIdProductResponse>> GetById(Guid id, CancellationToken ct = default)
    {
        var result = await getByIdProductQuery.ExecuteAsync(id, ct);
        return result.Match<ActionResult<GetByIdProductResponse>>(
                product => Ok(product),
                error => NotFound(error.Message)
                );
    }
}