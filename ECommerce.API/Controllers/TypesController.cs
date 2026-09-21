using ECommerce.API.Contracts.Responses;
using ECommerce.UseCases.Features.ProductTypes.Responses;
using ECommerce.UseCases.Features.ProductTypes.Queries.GetTypes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ECommerce.API.Constants;
using ECommerce.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using ECommerce.UseCases.Features.ProductTypes.Commands.DeleteType;
using ECommerce.UseCases.Features.ProductTypes.Commands.UpdateType;
using ECommerce.UseCases.Features.ProductTypes.Commands.CreateType;
using ECommerce.UseCases.Features.ProductTypes.Queries.GetTypeById;

namespace ECommerce.API.Controllers;

/// <summary>
/// API Controller responsible for managing product types within the catalog.
/// Provides endpoints for retrieving product types as well as administrative operations (Create, Update, Delete).
/// </summary>
public class TypesController(IMediator mediator) : ApiControllerBase
{
    /// <summary>
    /// Get all types
    /// </summary>
    /// <param name="query">The query parameters for filtering (e.g., search term) and sorting.</param>
    /// <param name="ct">A CancellationToken used to cancel the request</param>
    /// <returns>
    /// Returns a list of types with their Id and Name
    /// </returns>
    /// <response code="200">Types returned successfully</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<TypeResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<TypeResponse>>>> GetAll(
        [FromQuery] GetTypesQuery query,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(query, ct);
        if (result.IsFailure)
            return Problem(result);

        return Success(result.Value, TypeMessages.ListRetrievedSuccessfully);
    }
    /// <summary>
    /// Gets a type by its Id
    /// </summary>
    /// <param name="id">The unique identifier of the type</param>
    /// <param name="ct">A CancellationToken used to cancel the request</param>
    /// <returns>
    /// Returns the type details if found, otherwise returns a 404 Not Found response
    /// </returns>
    /// <response code="200">Type was found successfully</response>
    /// <response code="400">Invalid validation or bad request</response>
    /// <response code="404">Type was not found</response>
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<TypeResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<TypeResponse>>> GetById(Guid id, CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetTypeByIdQuery(id), ct);
        if (result.IsFailure)
            return Problem(result);

        return Success(result.Value, TypeMessages.RetrievedSuccessfully);
    }

    /// <summary>
    /// Creates a new type
    /// </summary>
    /// <param name="request">The type details needed for creation</param>
    /// <param name="ct">A CancellationToken used to cancel the request</param>
    /// <returns>
    /// Returns the unique identifier of the newly created type
    /// </returns>
    /// <response code="201">Type created successfully</response>
    /// <response code="400">Invalid validation or bad request</response>
    /// <response code="401">Unauthorized user</response>
    /// <response code="403">Forbidden. User does not have the required role</response>
    /// <response code="409">A type with the new name already exists</response>
    [HttpPost]
    [Authorize(Roles = $"{Roles.Admin},{Roles.SuperAdmin}")]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<Guid>>> Create(
        [FromBody] CreateTypeCommand request,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(request, ct);
        if (result.IsFailure)
            return Problem(result);

        var response = ApiResponse<Guid>.Ok(
            result.Value,
            HttpContext.TraceIdentifier,
            TypeMessages.CreatedSuccessfully);

        return CreatedAtAction(
            actionName: nameof(GetById),
            routeValues: new { id = result.Value },
            value: response);
    }

    /// <summary>
    /// Updates an existing type by its Id
    /// </summary>
    /// <param name="id">The unique identifier of the type to update</param>
    /// <param name="request">The updated type details</param>
    /// <param name="ct">A CancellationToken used to cancel the request</param>
    /// <returns>
    /// Returns a success message if updated successfully
    /// </returns>
    /// <response code="200">Type updated successfully</response>
    /// <response code="400">Invalid validation or bad request</response>
    /// <response code="401">Unauthorized user</response>
    /// <response code="403">Forbidden. User does not have the required role</response>
    /// <response code="404">Type was not found</response>
    /// <response code="409">A type with the new name already exists</response>
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
        [FromBody] UpdateTypeCommand request,
        CancellationToken ct = default)
    {
        var command = request with { Id = id };

        var result = await mediator.Send(command, ct);
        if (result.IsFailure)
            return Problem(result);

        return Success(TypeMessages.UpdatedSuccessfully);
    }

    /// <summary>
    /// Deletes a type by its Id
    /// </summary>
    /// <param name="id">The unique identifier of the type to delete</param>
    /// <param name="ct">A CancellationToken used to cancel the request</param>
    /// <returns>
    /// Returns a success message if deleted successfully
    /// </returns>
    /// <response code="200">Type deleted successfully</response>
    /// <response code="400">Invalid request</response>
    /// <response code="404">Type was not found</response>
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
        var result = await mediator.Send(new DeleteTypeCommand(id), ct);
        if (result.IsFailure)
            return Problem(result);

        return Success(TypeMessages.DeletedSuccessfully);
    }

}
