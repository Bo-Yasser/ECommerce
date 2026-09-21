using ECommerce.API.Contracts.Responses;
using ECommerce.UseCases.Features.Products.Queries.GetPagedProducts;
using ECommerce.UseCases.Features.Products.Responses;
using ECommerce.UseCases.Features.Products.Queries.GetProductById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ECommerce.API.Constants;
using ECommerce.UseCases.Features.Products.Commands.CreateProduct;
using ECommerce.UseCases.Features.Products.Commands.UpdateProduct;
using ECommerce.UseCases.Features.Products.Commands.DeleteProduct;
using Microsoft.AspNetCore.Authorization;
using ECommerce.Domain.Constants;

namespace ECommerce.API.Controllers;

/// <summary>
/// API Controller responsible for managing the products catalog.
/// Provides endpoints for retrieving product as well as administrative operations (Create, Update, Delete).
/// </summary>
public class ProductsController(IMediator mediator) : ApiControllerBase
{
    /// <summary>
    /// Get a paginated list of all products
    /// </summary>
    /// <param name="query">A query used to specify product pagination and filtering</param>
    /// <param name="ct">A CancellationToken used to cancel the request</param>
    /// <returns>
    /// Returns a paginated list of products
    /// </returns>
    /// <response code="200">Products returned successfully</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<ProductResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<ProductResponse>>>> Paged(
        [FromQuery] GetPagedProductsQuery query,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(query, ct);
        if (result.IsFailure)
            return Problem(result);

        return FromPagedResult(result, query.PageNumber, query.PageSize, ProductMessages.ListRetrievedSuccessfully);
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
    /// <response code="400">Invalid validation or bad request</response>
    /// <response code="404">Product was not found</response>
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<ProductResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<ProductResponse>>> GetById(Guid id, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetProductByIdQuery(id), ct);
        if (result.IsFailure)
            return Problem(result);

        return Success(result.Value, ProductMessages.RetrievedSuccessfully);
    }

    /// <summary>
    /// Creates a new product
    /// </summary>
    /// <param name="request">The product details needed for creation</param>
    /// <param name="ct">A CancellationToken used to cancel the request</param>
    /// <returns>
    /// Returns the unique identifier of the newly created product
    /// </returns>
    /// <response code="201">Product created successfully</response>
    /// <response code="400">Invalid validation or bad request</response>
    /// <response code="409">A product with the same name already exists</response>
    /// <response code="401">Unauthorized user</response>
    /// <response code="403">Forbidden. User does not have the required role</response>
    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.SuperAdmin}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<Guid>>> Create(
        [FromBody] CreateProductCommand request,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(request, ct);
        if (result.IsFailure)
            return Problem(result);

        var response = ApiResponse<Guid>.Ok(
            result.Value,
            HttpContext.TraceIdentifier,
            ProductMessages.CreatedSuccessfully);

        return CreatedAtAction(
            actionName: nameof(GetById),
            routeValues: new { id = result.Value },
            value: response);
    }

    /// <summary>
    /// Updates an existing product by its Id
    /// </summary>
    /// <param name="id">The unique identifier of the product to update</param>
    /// <param name="request">The updated product details</param>
    /// <param name="ct">A CancellationToken used to cancel the request</param>
    /// <returns>
    /// Returns a success message if updated successfully
    /// </returns>
    /// <response code="200">Product updated successfully</response>
    /// <response code="400">Invalid validation or bad request</response>
    /// <response code="404">Product was not found</response>
    /// <response code="409">A product with the new name already exists</response>
    /// <response code="401">Unauthorized user</response>
    /// <response code="403">Forbidden. User does not have the required role</response>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.SuperAdmin}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateProductCommand request,
        CancellationToken ct = default)
    {
        var command = request with { Id = id };

        var result = await mediator.Send(command, ct);
        if (result.IsFailure)
            return Problem(result);

        return Success(ProductMessages.UpdatedSuccessfully);
    }

    /// <summary>
    /// Deletes a product by its Id
    /// </summary>
    /// <param name="id">The unique identifier of the product to delete</param>
    /// <param name="ct">A CancellationToken used to cancel the request</param>
    /// <returns>
    /// Returns a success message if deleted successfully
    /// </returns>
    /// <response code="200">Product deleted successfully</response>
    /// <response code="400">Invalid request</response>
    /// <response code="404">Product was not found</response>
    /// <response code="401">Unauthorized user</response>
    /// <response code="403">Forbidden. User does not have the required role</response>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = $"{Roles.Admin},{Roles.SuperAdmin}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        Guid id,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(new DeleteProductCommand(id), ct);
        if (result.IsFailure)
            return Problem(result);

        return Success(ProductMessages.DeletedSuccessfully);
    }
}