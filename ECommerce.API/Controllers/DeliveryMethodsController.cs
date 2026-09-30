using ECommerce.API.Constants;
using ECommerce.API.Contracts.Requests.DeliveryMethods;
using ECommerce.API.Contracts.Responses;
using ECommerce.Domain.Constants;
using ECommerce.UseCases.Features.DeliveryMethods.Commands.CreateDeliveryMethod;
using ECommerce.UseCases.Features.DeliveryMethods.Commands.DeleteDeliveryMethod;
using ECommerce.UseCases.Features.DeliveryMethods.Commands.UpdateDeliveryMethod;
using ECommerce.UseCases.Features.DeliveryMethods.Queries.GetDeliveryMethodById;
using ECommerce.UseCases.Features.DeliveryMethods.Queries.GetDeliveryMethods;
using ECommerce.UseCases.Features.DeliveryMethods.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;


/// <summary>
/// Manages delivery methods available in the e-commerce system.
/// </summary>
public class DeliveryMethodsController(ISender sender) : ApiControllerBase
{
    /// <summary>
    /// Retrieves a list of all delivery methods based on the provided filtering criteria.
    /// </summary>
    /// <param name="request">The query parameters for filtering (e.g., search term, availability) and sorting.</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>An API response containing a read-only list of delivery methods.</returns>
    /// <response code="200">The list of delivery methods was retrieved successfully.</response>
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<DeliveryMethodResponse>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<DeliveryMethodResponse>>>> GetAll(
        [FromQuery] GetDeliveryMethodsQuery request,
        CancellationToken ct = default)
    {
        var result = await sender.Send(request, ct);

        if (result.IsFailure)
            return Problem(result);

        return Success(result.Value, DeliveryMethodMessages.DeliveryMethodsRetrievedSuccessfully);
    }

    /// <summary>
    /// Retrieves a specific delivery method by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the delivery method.</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>An API response containing the delivery method details.</returns>
    /// <response code="200">The delivery method was found and retrieved successfully.</response>
    /// <response code="400">Invalid validation or bad request</response>
    /// <response code="404">No delivery method was found with the specified Id.</response>
    [HttpGet("{id:guid}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ApiResponse<DeliveryMethodResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<DeliveryMethodResponse>>> GetById(
        Guid id,
        CancellationToken ct = default)
    {
        var result = await sender.Send(new GetDeliveryMethodByIdQuery(id), ct);

        if (result.IsFailure)
            return Problem(result);

        return Success(result.Value, DeliveryMethodMessages.DeliveryMethodRetrievedSuccessfully);
    }


    /// <summary>
    /// Creates a new delivery method in the system.
    /// </summary>
    /// <param name="request">The payload containing the delivery method details (e.g., name, estimated time, description, price).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>An API response containing the unique identifier of the newly created delivery method.</returns>
    /// <response code="201">The delivery method was created successfully.</response>
    /// <response code="400">Invalid validation or bad request</response>
    /// <response code="401">Unauthorized user</response>
    /// <response code="403">Forbidden. User does not have the required role</response>
    /// <response code="409">A conflict occurred, such as a name duplication with another delivery method.</response>
    [HttpPost]
    [Authorize(Roles = Roles.SuperAdmin)]
    [ProducesResponseType(typeof(ApiResponse<Guid>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<Guid>>> Create(
        [FromBody] CreateDeliveryMethodCommand request,
        CancellationToken ct = default)
    {
        var result = await sender.Send(request, ct);

        if (result.IsFailure)
            return Problem(result);

        var response = ApiResponse<Guid>.Ok(
            result.Value,
            HttpContext.TraceIdentifier,
            DeliveryMethodMessages.DeliveryMethodCreatedSuccessfully);

        return CreatedAtAction(
           actionName: nameof(GetById),
           routeValues: new {id = result.Value},
           value: response);
    }


    /// <summary>
    /// Updates an existing delivery method with new details.
    /// </summary>
    /// <param name="id">The unique identifier of the delivery method to update.</param>
    /// <param name="request">The payload containing the updated details for the delivery method.</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>An API response indicating the delivery method was updated successfully.</returns>
    /// <response code="200">The delivery method was updated successfully.</response>
    /// <response code="400">Invalid validation or bad request</response>
    /// <response code="401">Unauthorized user</response>
    /// <response code="403">Forbidden. User does not have the required role</response>
    /// <response code="404">No delivery method was found with the specified Id.</response>
    /// <response code="409">A conflict occurred, such as a name duplication with another delivery method or a concurrency conflict.</response>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = Roles.SuperAdmin)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateDeliveryMethodRequest request,
        CancellationToken ct = default)
    {
        var command = new UpdateDeliveryMethodCommand(
            id,
            request.Name,
            request.Price,
            request.EstimatedDeliveryTime,
            request.Description,
            request.IsAvailable,
            request.DisplayOrder,
            request.RowVersion);

        var result = await sender.Send(command, ct);
        if (result.IsFailure)
            return Problem(result);

        return Success(DeliveryMethodMessages.DeliveryMethodUpdatedSuccessfully);
    }


    /// <summary>
    /// Deletes a specific delivery method from the system.
    /// </summary>
    /// <param name="id">The unique identifier of the delivery method to delete.</param>
    /// <param name="request">
    /// The request containing the expected row version of the delivery method.
    /// </param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>An API response indicating the delivery method was deleted successfully.</returns>
    /// <response code="200">The delivery method was deleted successfully.</response>
    /// <response code="400">Invalid validation or bad request</response>
    /// <response code="401">Unauthorized user</response>
    /// <response code="403">Forbidden. User does not have the required role</response>
    /// <response code="404">No delivery method was found with the specified Id.</response>
    /// <response code="409">The delivery method could not be deleted because of a concurrency conflict.</response>
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = Roles.SuperAdmin)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(
        Guid id,
        [FromBody] DeleteDeliveryMethodRequest request,
        CancellationToken ct = default)
    {
        var result = await sender.Send(new DeleteDeliveryMethodCommand(id, request.RowVersion), ct);

        if (result.IsFailure)
            return Problem(result);

        return Success(DeliveryMethodMessages.DeliveryMethodDeletedSuccessfully);
    }
}
