using ECommerce.API.Constants;
using ECommerce.API.Contracts.Responses;
using ECommerce.Domain.Constants;
using ECommerce.UseCases.Features.ProductBrands.Commands.CreateBrand;
using ECommerce.UseCases.Features.ProductBrands.Commands.DeleteBrand;
using ECommerce.UseCases.Features.ProductBrands.Commands.UpdateBrand;
using ECommerce.UseCases.Features.ProductBrands.Queries.GetBrandById;
using ECommerce.UseCases.Features.ProductBrands.Queries.GetBrands;
using ECommerce.UseCases.Features.ProductBrands.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

/// <summary>
/// API Controller responsible for managing product brands within the catalog.
/// Provides endpoints for retrieving product brands as well as administrative operations (Create, Update, Delete).
/// </summary>
public class BrandsController(IMediator mediator) : ApiControllerBase
{
    /// <summary>
    /// Get all brands
    /// </summary>
    /// <param name="query">The query parameters for filtering (e.g., search term) and sorting.</param>
    /// <param name="ct">A CancellationToken used to cancel the request</param>
    /// <returns>
    /// Returns a list of brands with their Id and Name
    /// </returns>
    /// <response code="200">Brands returned successfully</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<BrandResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<BrandResponse>>>> GetAll(
        [FromQuery] GetBrandsQuery query,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(query, ct);
        if (result.IsFailure)
            return Problem(result);

        return Success(result.Value, BrandMessages.ListRetrievedSuccessfully);
    }

    /// <summary>
    /// Gets a brand by its Id
    /// </summary>
    /// <param name="id">The unique identifier of the brand</param>
    /// <param name="ct">A CancellationToken used to cancel the request</param>
    /// <returns>
    /// Returns the brand details if found, otherwise returns a 404 Not Found response
    /// </returns>
    /// <response code="200">Brand was found successfully</response>
    /// <response code="400">Invalid validation or bad request</response>
    /// <response code="404">Brand was not found</response>
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<BrandResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<BrandResponse>>> GetById(Guid id, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetBrandByIdQuery(id), ct);
        if (result.IsFailure)
            return Problem(result);

        return Success(result.Value, BrandMessages.RetrievedSuccessfully);
    }

    /// <summary>
    /// Creates a new brand
    /// </summary>
    /// <param name="request">The brand details needed for creation</param>
    /// <param name="ct">A CancellationToken used to cancel the request</param>
    /// <returns>
    /// Returns the unique identifier of the newly created brand
    /// </returns>
    /// <response code="201">Brand created successfully</response>
    /// <response code="400">Invalid validation or bad request</response>
    /// <response code="401">Unauthorized user</response>
    /// <response code="403">Forbidden. User does not have the required role</response>
    /// <response code="409">A brand with the same name already exists</response>
    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.SuperAdmin}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<Guid>>> Create(
        [FromBody] CreateBrandCommand request,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(request, ct);
        if (result.IsFailure)
            return Problem(result);

        var response = ApiResponse<Guid>.Ok(
            result.Value,
            HttpContext.TraceIdentifier,
            BrandMessages.CreatedSuccessfully);

        return CreatedAtAction(
            actionName: nameof(GetById),
            routeValues: new { id = result.Value },
            value: response);
    }

    /// <summary>
    /// Updates an existing brand by its Id
    /// </summary>
    /// <param name="id">The unique identifier of the brand to update</param>
    /// <param name="request">The updated brand details</param>
    /// <param name="ct">A CancellationToken used to cancel the request</param>
    /// <returns>
    /// Returns a success message if updated successfully
    /// </returns>
    /// <response code="200">Brand updated successfully</response>
    /// <response code="400">Invalid validation or bad request</response>
    /// <response code="401">Unauthorized user</response>
    /// <response code="403">Forbidden. User does not have the required role</response>
    /// <response code="404">Brand was not found</response>
    /// <response code="409">A brand with the new name already exists</response>
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
        [FromBody] UpdateBrandCommand request,
        CancellationToken ct = default)
    {
        var command = request with { Id = id };

        var result = await mediator.Send(command, ct);
        if (result.IsFailure)
            return Problem(result);

        return Success(BrandMessages.UpdatedSuccessfully);
    }

    /// <summary>
    /// Deletes a brand by its Id
    /// </summary>
    /// <param name="id">The unique identifier of the brand to delete</param>
    /// <param name="ct">A CancellationToken used to cancel the request</param>
    /// <returns>
    /// Returns a success message if deleted successfully
    /// </returns>
    /// <response code="200">Brand deleted successfully</response>
    /// <response code="400">Invalid request</response>
    /// <response code="404">Brand was not found</response>
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
        var result = await mediator.Send(new DeleteBrandCommand(id), ct);
        if (result.IsFailure)
            return Problem(result);

        return Success(BrandMessages.DeletedSuccessfully);
    }
}