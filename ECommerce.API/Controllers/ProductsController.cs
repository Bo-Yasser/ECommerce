using ECommerce.API.Contracts.Responses;
using ECommerce.UseCases.Products.Queries.GetPagedProducts;
using ECommerce.UseCases.Products.Queries.GetProductById;
using ECommerce.UseCases.Products.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

public class ProductsController(IMediator mediator) : ApiControllerBase
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
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<GetProductsResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<GetProductsResponse>>>> Paged(
        [FromQuery] GetPagedProductsQuery query,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(query, ct);
        if (result.IsFailure)
            return Problem(result);

        return FromPagedResult(result, query.PageNumber, query.PageSize, "Paged Products Retrieved Successfully");
    }

    /// <summary>
    /// Gets a product by its Id
    /// </summary>
    /// <param name="id">The unique identifier of   the product</param>
    /// <param name="ct">A CancellationToken used to cancel the request</param>
    /// <returns>
    /// Returns the product details if found, otherwise returns a 404 Not Found response
    /// </returns>
    /// <response code="200">Product was found successfully</response>
    /// <response code="404">Product was not found</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<GetProductByIdResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<GetProductByIdResponse>>> GetById(Guid id, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetProductByIdQuery(id), ct);
        if (result.IsFailure)
            return Problem(result);

        return Ok(ApiResponse<GetProductByIdResponse>.Ok(result.Value, HttpContext.TraceIdentifier));
    }
}