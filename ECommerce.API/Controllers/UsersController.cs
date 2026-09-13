using ECommerce.API.Constants;
using ECommerce.API.Contracts.Responses;
using ECommerce.UseCases.Features.Users.Commands.AddUserAddress;
using ECommerce.UseCases.Features.Users.Queries.GetCurrentUser;
using ECommerce.UseCases.Features.Users.Queries.GetUserAddresses;
using ECommerce.UseCases.Features.Users.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;


/// <summary>
/// Manages operations related to the currently authenticated user's profile and addresses.
/// </summary>
[Authorize]
public class UsersController(IMediator mediator) : ApiControllerBase
{
    /// <summary>
    /// Retrieves the profile details of the currently authenticated user.
    /// </summary>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>An API response containing the user's profile information.</returns>
    /// <response code="200">The user profile was retrieved successfully.</response>
    /// <response code="401">The user is not authenticated or the access token is missing/invalid.</response>
    [HttpGet("me")]
    [ProducesResponseType(typeof(ApiResponse<UserProfileResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<UserProfileResponse>>> GetCurrentUser(CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetCurrentUserQuery(), ct);
        if(result.IsFailure)
            return Problem(result);

        return Success(result.Value, UserMessages.ProfileRetrievedSuccessfully);
    }

    /// <summary>
    /// Retrieves all saved shipping addresses for the currently authenticated user.
    /// </summary>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>An API response containing a list of the user's addresses.</returns>
    /// <response code="200">The user's addresses were retrieved successfully (returns an empty list if none exist).</response>
    /// <response code="401">The user is not authenticated or the access token is missing/invalid.</response>
    [HttpGet("me/addresses")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<UserAddressResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<UserAddressResponse>>>> GetUserAddresses(CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetUserAddressesQuery(), ct);
        if (result.IsFailure)
            return Problem(result);

        return Success(result.Value, UserMessages.AddressesRetrievedSuccessfully);
    }

    /// <summary>
    /// Adds a new shipping address to the currently authenticated user's profile.
    /// </summary>
    /// <param name="request">The payload containing the address details (e.g., street, city, country, postal code).</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>An API response containing the newly created user address with its assigned ID.</returns>
    /// <response code="201">The address was successfully added to the user's profile.</response>
    /// <response code="400">The request payload is invalid or fails validation rules.</response>
    /// <response code="401">The user is not authenticated or the access token is missing/invalid.</response>
    [HttpPost("me/addresses")]
    [ProducesResponseType(typeof(ApiResponse<UserAddressResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<UserAddressResponse>>> CreateUserAddress(
        [FromBody] AddUserAddressCommand request,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(request, ct);
        if (result.IsFailure)
            return Problem(result);

        return Created(string.Empty, ApiResponse<UserAddressResponse>.Ok(
            result.Value,
            HttpContext.TraceIdentifier,
            UserMessages.AddressAddedSuccessfully));
    }
}
