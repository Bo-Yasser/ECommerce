using ECommerce.API.Constants;
using ECommerce.API.Contracts.Requests.Orders;
using ECommerce.API.Contracts.Responses;
using ECommerce.Domain.Constants;
using ECommerce.UseCases.Features.Orders.Commands.CancelOrder;
using ECommerce.UseCases.Features.Orders.Commands.CreateOrder;
using ECommerce.UseCases.Features.Orders.Commands.DeliverOrder;
using ECommerce.UseCases.Features.Orders.Commands.ProcessOrder;
using ECommerce.UseCases.Features.Orders.Commands.ShipOrder;
using ECommerce.UseCases.Features.Orders.Models;
using ECommerce.UseCases.Features.Orders.Queries.GetOrderById;
using ECommerce.UseCases.Features.Orders.Queries.GetOrderByIdForUser;
using ECommerce.UseCases.Features.Orders.Queries.GetPagedOrders;
using ECommerce.UseCases.Features.Orders.Queries.GetPagedOrdersByUserId;
using ECommerce.UseCases.Features.Orders.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

/// <summary>
/// Manages customer orders within the e-commerce system.
/// Provides administrative endpoints for viewing and managing orders,
/// as well as authenticated customer endpoints for creating, retrieving, and cancelling their own orders.
/// </summary>
public class OrdersController(ISender sender) : ApiControllerBase
{

    /// <summary>
    /// Gets a paginated list of all orders for administrative management.
    /// Supports filtering, sorting, and pagination.
    /// </summary>
    /// <param name="request">
    /// The query parameters used to specify pagination, filtering, sorting,
    /// and optionally filter orders by a specific user.
    /// </param>
    /// <param name="ct">
    /// A cancellation token to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// Returns a paginated list of orders matching the specified criteria.
    /// </returns>
    /// <response code="200">Orders were retrieved successfully.</response>
    /// <response code="400">The request parameters are invalid.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user does not have the required administrative role.</response>
    [HttpGet]
    [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Admin}")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<OrderResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<OrderResponse>>>> PagedOrders(
        [FromQuery] GetPagedOrdersQuery request,
        CancellationToken ct = default)
    {
        var result = await sender.Send(request, ct);

        if (result.IsFailure)
            return Problem(result);

        return FromPagedResult(result, request.PageNumber, request.PageSize, OrderMessages.OrdersRetrievedSuccessfully);
    }

    /// <summary>
    /// Gets a paginated list of orders belonging to the currently authenticated user.
    /// Supports filtering, sorting, and pagination.
    /// </summary>
    /// <param name="request">
    /// The query parameters used to specify pagination, filtering, and sorting options.
    /// The user identity is determined from the authenticated user context.
    /// </param>
    /// <param name="ct">
    /// A cancellation token to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// Returns a paginated list of orders belonging to the current authenticated user.
    /// </returns>
    /// <response code="200">The user's orders were retrieved successfully.</response>
    /// <response code="400">The request parameters are invalid.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">Access to the resource is forbidden.</response>
    [HttpGet("me")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<OrderResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<OrderResponse>>>> PagedOrdersForUser(
        [FromQuery] GetPagedOrdersByUserIdRequest request,
        CancellationToken ct = default)
    {
        var query = new GetPagedOrdersByUserIdQuery(
            PageNumber: request.PageNumber,
            PageSize: request.PageSize,
            new OrderFilters(
                Search: request.Search,
                OrderId: request.OrderId,
                ProductId: request.ProductId,
                ProductName: request.ProductName,
                ProductSku: request.ProductSku,
                DeliveryMethodId: request.DeliveryMethodId,
                Status: request.Status),
            SortBy: request.SortBy,
            SortDescending: request.SortDescending);

        var result = await sender.Send(query, ct);

        if (result.IsFailure)
            return Problem(result);

        return FromPagedResult(result, request.PageNumber, request.PageSize, OrderMessages.OrdersRetrievedSuccessfully);
    }

    /// <summary>
    /// Gets a specific order by its Id for administrative management.
    /// </summary>
    /// <param name="id">The unique identifier of the order to retrieve.</param>
    /// <param name="ct">
    /// A cancellation token to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// Returns the requested order details if the order exists.
    /// </returns>
    /// <response code="200">The order was retrieved successfully.</response>
    /// <response code="400">The provided order Id is invalid.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user does not have the required administrative role.</response>
    /// <response code="404">The specified order was not found.</response>
    [HttpGet("{id:guid}")]
    [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Admin}")]
    [ProducesResponseType(typeof(ApiResponse<OrderResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<OrderResponse>>> GetOrderById(
        Guid id,
        CancellationToken ct = default)
    {
        var result = await sender.Send(new GetOrderByIdQuery(id), ct);

        if (result.IsFailure)
            return Problem(result);

        return Success(result.Value, OrderMessages.OrderRetrievedSuccessfully);
    }

    /// <summary>
    /// Gets a specific order by its Id for the currently authenticated user.
    /// The returned order must belong to the current user.
    /// </summary>
    /// <param name="id">The unique identifier of the order to retrieve.</param>
    /// <param name="ct">
    /// A cancellation token to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// Returns the requested order details if it belongs to the current authenticated user.
    /// </returns>
    /// <response code="200">The order was retrieved successfully.</response>
    /// <response code="400">The provided order Id is invalid.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">Access to the resource is forbidden.</response>
    /// <response code="404">The specified order was not found or does not belong to the current user.</response>
    [HttpGet("me/{id:guid}")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<OrderResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ApiResponse<OrderResponse>>> GetOrderByIdForUser(
        Guid id,
        CancellationToken ct = default)
    {
        var result = await sender.Send(new GetOrderByIdForUserQuery(id), ct);

        if (result.IsFailure)
            return Problem(result);

        return Success(result.Value, OrderMessages.OrderRetrievedSuccessfully);
    }

    /// <summary>
    /// Creates a new order for the currently authenticated user using the current shopping basket.
    /// </summary>
    /// <param name="request">
    /// The command containing the information required to create the order.
    /// </param>
    /// <param name="ct">
    /// A cancellation token to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// Returns the newly created order.
    /// </returns>
    /// <response code="201">The order was created successfully.</response>
    /// <response code="400">The request is invalid or the order cannot be created.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user is not allowed to create an order.</response>
    /// <response code="404">A required resource such as the basket, product, delivery method, or address was not found.</response>
    /// <response code="409">The order could not be created because of a concurrency conflict.</response>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<OrderResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<OrderResponse>>> Create(
        [FromBody] CreateOrderCommand request,
        CancellationToken ct = default)
    {
        var result = await sender.Send(request, ct);

        if (result.IsFailure)
            return Problem(result);

        var response = ApiResponse<OrderResponse>.Ok(
            result.Value,
            HttpContext.TraceIdentifier,
            OrderMessages.OrderCreatedSuccessfully);

        return CreatedAtAction(
            actionName: nameof(GetOrderByIdForUser),
            routeValues: new { id = result.Value.Id },
            value: response);
    }

    /// <summary>
    /// Cancels a pending order belonging to the currently authenticated user.
    /// Requires the expected row version to ensure the order has not been modified by another process.
    /// </summary>
    /// <param name="id">The unique identifier of the order to cancel.</param>
    /// <param name="request">
    /// The request containing the expected row version of the order.
    /// </param>
    /// <param name="ct">
    /// A cancellation token to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// Returns a success response when the order is cancelled successfully.
    /// </returns>
    /// <response code="200">The order was cancelled successfully.</response>
    /// <response code="400">The request or row version is invalid, or the order cannot be cancelled.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user is not allowed to cancel the specified order.</response>
    /// <response code="404">The specified order was not found.</response>
    /// <response code="409">The order has been modified since the supplied row version was retrieved.</response>
    [HttpPost("{id:guid}/cancel")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Cancel(
        Guid id,
        [FromBody] OrderConcurrencyRequest request,
        CancellationToken ct = default)
    {
        var result = await sender.Send(
            new CancelOrderCommand(id, request.RowVersion),
            ct);

        if (result.IsFailure)
            return Problem(result);

        return Success(OrderMessages.OrderCancelledSuccessfully);
    }

    /// <summary>
    /// Moves an order from Pending to Processing as part of the order fulfillment workflow.
    /// </summary>
    /// <param name="id">The unique identifier of the order to process.</param>
    /// <param name="request">
    /// The request containing the expected row version of the order.
    /// </param>
    /// <param name="ct">
    /// A cancellation token to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// Returns a success response when the order is successfully moved to Processing.
    /// </returns>
    /// <response code="200">The order was processed successfully.</response>
    /// <response code="400">The request or row version is invalid, or the order cannot be processed.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user does not have the required administrative role.</response>
    /// <response code="404">The specified order was not found.</response>
    /// <response code="409">The order has been modified since the supplied row version was retrieved.</response>
    [HttpPost("{id:guid}/process")]
    [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Admin}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Process(
        Guid id,
        [FromBody] OrderConcurrencyRequest request,
        CancellationToken ct = default)
    {
        var result = await sender.Send(
            new ProcessOrderCommand(id, request.RowVersion),
            ct);

        if (result.IsFailure)
            return Problem(result);

        return Success(OrderMessages.OrderProcessedSuccessfully);
    }

    /// <summary>
    /// Moves an order from Processing to Shipped as part of the order fulfillment workflow.
    /// </summary>
    /// <param name="id">The unique identifier of the order to ship.</param>
    /// <param name="request">
    /// The request containing the expected row version of the order.
    /// </param>
    /// <param name="ct">
    /// A cancellation token to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// Returns a success response when the order is successfully moved to Shipped.
    /// </returns>
    /// <response code="200">The order was shipped successfully.</response>
    /// <response code="400">The request or row version is invalid, or the order cannot be shipped.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user does not have the required administrative role.</response>
    /// <response code="404">The specified order was not found.</response>
    /// <response code="409">The order has been modified since the supplied row version was retrieved.</response>
    [HttpPost("{id:guid}/ship")]
    [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Admin}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Ship(
        Guid id,
        [FromBody] OrderConcurrencyRequest request,
        CancellationToken ct = default)
    {
        var result = await sender.Send(
            new ShipOrderCommand(id, request.RowVersion),
            ct);

        if (result.IsFailure)
            return Problem(result);

        return Success(OrderMessages.OrderShippedSuccessfully);
    }

    /// <summary>
    /// Moves an order from Shipped to Delivered as part of the order fulfillment workflow.
    /// </summary>
    /// <param name="id">The unique identifier of the order to deliver.</param>
    /// <param name="request">
    /// The request containing the expected row version of the order.
    /// </param>
    /// <param name="ct">
    /// A cancellation token to observe while waiting for the task to complete.
    /// </param>
    /// <returns>
    /// Returns a success response when the order is successfully moved to Delivered.
    /// </returns>
    /// <response code="200">The order was delivered successfully.</response>
    /// <response code="400">The request or row version is invalid, or the order cannot be delivered.</response>
    /// <response code="401">The user is not authenticated.</response>
    /// <response code="403">The user does not have the required administrative role.</response>
    /// <response code="404">The specified order was not found.</response>
    /// <response code="409">The order has been modified since the supplied row version was retrieved.</response>
    [HttpPost("{id:guid}/deliver")]
    [Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Admin}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Deliver(
        Guid id,
        [FromBody] OrderConcurrencyRequest request,
        CancellationToken ct = default)
    {
        var result = await sender.Send(
            new DeliverOrderCommand(id, request.RowVersion),
            ct);

        if (result.IsFailure)
            return Problem(result);

        return Success(OrderMessages.OrderDeliveredSuccessfully);
    }

}
