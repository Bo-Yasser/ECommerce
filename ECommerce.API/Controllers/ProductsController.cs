using ECommerce.API.Models;
using ECommerce.UseCases.Products.Dtos;
using ECommerce.UseCases.Products.Queries;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

public class ProductsController(
    GetAllProductsQuery getAllProductsQuery,
    GetByIdProductQuery getByIdProductQuery) : ApiControllerBase
{
    /// <summary>
    /// Get all products
    /// </summary>
    /// <param name="ct">A CancellationToken used to cancel the request</param>
    /// <returns>
    /// Returns a list of products with their Id and Name
    /// </returns>
    /// <response code="200">Products returned successfully</response>
    [HttpGet] // GET api/products
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<GetAllProductsResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<GetAllProductsResponse>>>> GetAll(CancellationToken ct = default)
    {
        var result = await getAllProductsQuery.ExecuteAsync(ct);
        if (result.IsFailure)
            return Problem(result);
        return Ok(ApiResponse<IReadOnlyList<GetAllProductsResponse>>.Ok(result.Value, HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Gets a product by its Id
    /// </summary>
    /// <param name="id">The unique identifier of the product</param>
    /// <param name="ct">A CancellationToken used to cancel the request</param>
    /// <returns>
    /// Returns the product details if found, otherwise returns a 404 Not Found response
    /// </returns>
    /// <response code="200">Product was found successfully</response>
    /// <response code="404">Product was not found</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<GetByIdProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<GetByIdProductResponse>>> GetById(Guid id, CancellationToken ct = default)
    {
        var result = await getByIdProductQuery.ExecuteAsync(id, ct);
        if (result.IsFailure)
            return Problem(result);

        return Ok(ApiResponse<GetByIdProductResponse>.Ok(result.Value, HttpContext.TraceIdentifier));
    }
}