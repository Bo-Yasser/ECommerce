using ECommerce.API.Constants;
using ECommerce.API.Contracts.Requests.Basket;
using ECommerce.API.Contracts.Responses;
using ECommerce.API.Extensions;
using ECommerce.API.Filters;
using ECommerce.UseCases.Features.Basket.Commands.AddBasketItem;
using ECommerce.UseCases.Features.Basket.Commands.ClearBasket;
using ECommerce.UseCases.Features.Basket.Commands.MergeBasket;
using ECommerce.UseCases.Features.Basket.Commands.RemoveBasketItem;
using ECommerce.UseCases.Features.Basket.Commands.UpdateBasketItemQuantity;
using ECommerce.UseCases.Features.Basket.Queries.GetBasket;
using ECommerce.UseCases.Features.Basket.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ServiceFilter(typeof(BuyerIdFilter))]
public class BasketController(IMediator mediator) : ApiControllerBase
{
    /// <summary>
    /// Retrieves the shopping basket for the current buyer. Creates a new empty basket if one does not exist.
    /// </summary>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The current state of the shopping basket including all items and the total price.</returns>
    /// <response code="200">The basket was retrieved or created successfully.</response>
    /// <response code="400">The request is invalid, or the Buyer Identifier is missing/malformed.</response>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<GetBasketResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<GetBasketResponse>>> Get(CancellationToken ct = default)
    {
        var buyerId = this.GetValidatedBuyerId();

        var result = await mediator.Send(new GetBasketQuery(buyerId), ct);
        if (result.IsFailure)
            return Problem(result);

        return Ok(ApiResponse<GetBasketResponse>.Ok(
            result.Value,
            HttpContext.TraceIdentifier,
            BasketMessages.RetrievedSuccessfully));
    }

    /// <summary>
    /// Adds a specific quantity of a product to the shopping basket. If the product already exists, the quantity is increased.
    /// </summary>
    /// <param name="request">The payload containing the Product Identifier and the Quantity to add.</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The updated state of the shopping basket after adding the item.</returns>
    /// <response code="200">The item was successfully added to the basket.</response>
    /// <response code="400">The request payload is invalid (e.g., negative quantity) or the Buyer Identifier is missing.</response>
    /// <response code="404">The specified product does not exist in the catalog.</response>
    [HttpPost("items")]
    [ProducesResponseType(typeof(ApiResponse<GetBasketResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<GetBasketResponse>>> AddItem(
        [FromBody] AddBasketItemRequest request,
        CancellationToken ct = default)
    {
        var buyerId = this.GetValidatedBuyerId();

        var result = await mediator.Send(
            new AddBasketItemCommand(buyerId, request.ProductId, request.Quantity),
            ct);
        if (result.IsFailure)
            return Problem(result);

        return Ok(ApiResponse<GetBasketResponse>.Ok(
            result.Value,
            HttpContext.TraceIdentifier,
            BasketMessages.ItemAddedSuccessfully));
    }

    /// <summary>
    /// Updates the exact quantity of an existing item in the shopping basket.
    /// </summary>
    /// <param name="productId">The unique identifier of the product currently in the basket.</param>
    /// <param name="request">The payload containing the new exact quantity.</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The updated state of the shopping basket after adjusting the quantity.</returns>
    /// <response code="200">The item quantity was successfully updated.</response>
    /// <response code="400">The request payload is invalid (e.g., zero or negative quantity).</response>
    /// <response code="404">The specified product was not found in the current basket.</response>
    [HttpPut("items/{productId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<GetBasketResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<GetBasketResponse>>> UpdateItemQuantity(
        Guid productId,
        [FromBody] UpdateBasketItemQuantityRequest request,
        CancellationToken ct = default)
    {
        var buyerId = this.GetValidatedBuyerId();

        var result = await mediator.Send(
            new UpdateBasketItemQuantityCommand(buyerId, productId, request.Quantity),
            ct);

        if (result.IsFailure)
            return Problem(result);

        return Ok(ApiResponse<GetBasketResponse>.Ok(
            result.Value,
            HttpContext.TraceIdentifier,
            BasketMessages.ItemQuantityUpdatedSuccessfully));
    }

    /// <summary>
    /// Removes a specific product completely from the shopping basket, regardless of its current quantity.
    /// </summary>
    /// <param name="productId">The unique identifier of the product to remove.</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The updated state of the shopping basket after removing the item.</returns>
    /// <response code="200">The item was successfully removed from the basket.</response>
    /// <response code="400">The Buyer Identifier is missing or malformed.</response>
    /// <response code="404">The specified product was not found in the current basket.</response>
    [HttpDelete("items/{productId:guid}")]
    [ProducesResponseType(typeof(ApiResponse<GetBasketResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<GetBasketResponse>>> RemoveItem(
        Guid productId,
        CancellationToken ct = default)
    {
        var buyerId = this.GetValidatedBuyerId();

        var result = await mediator.Send(
            new RemoveBasketItemCommand(buyerId, productId),
            ct);

        if (result.IsFailure)
            return Problem(result);

        return Ok(ApiResponse<GetBasketResponse>.Ok(
            result.Value,
            HttpContext.TraceIdentifier,
            BasketMessages.ItemRemovedSuccessfully));
    }

    /// <summary>
    /// Empties the shopping basket by removing all items, resetting the total price to zero.
    /// </summary>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The updated, empty state of the shopping basket.</returns>
    /// <response code="200">The basket was successfully cleared.</response>
    /// <response code="400">The Buyer Identifier is missing or malformed.</response>
    [HttpDelete]
    [ProducesResponseType(typeof(ApiResponse<GetBasketResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<GetBasketResponse>>> Clear(CancellationToken ct = default)
    {
        var buyerId = this.GetValidatedBuyerId();

        var result = await mediator.Send(
            new ClearBasketCommand(buyerId),
            ct);

        if (result.IsFailure)
            return Problem(result);

        return Ok(ApiResponse<GetBasketResponse>.Ok(
            result.Value,
            HttpContext.TraceIdentifier,
            BasketMessages.ClearedSuccessfully));
    }

    /// <summary>
    /// Merges the contents of a guest (anonymous) basket into the current authenticated user's basket.
    /// </summary>
    /// <param name="request">The payload containing the Anonymous Buyer Identifier.</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>The updated state of the authenticated user's basket after the merge operation.</returns>
    /// <response code="200">The anonymous basket was successfully merged into the target basket.</response>
    /// <response code="400">The request payload is invalid or the target Buyer Identifier is missing.</response>
    /// <response code="404">The specified anonymous basket could not be found.</response>
    [HttpPost("merge")]
    [ProducesResponseType(typeof(ApiResponse<GetBasketResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<GetBasketResponse>>> Merge(
        [FromBody] MergeBasketRequest request,
        CancellationToken ct = default)
    {
        var buyerId = this.GetValidatedBuyerId();

        var result = await mediator.Send(
            new MergeBasketCommand(buyerId, request.AnonymousBuyerId),
            ct);

        if (result.IsFailure)
            return Problem(result);

        return Ok(ApiResponse<GetBasketResponse>.Ok(
            result.Value,
            HttpContext.TraceIdentifier,
            BasketMessages.MergedSuccessfully));
    }
}