using ECommerce.API.Constants;
using ECommerce.API.Contracts.Requests.Stocks;
using ECommerce.API.Contracts.Responses;
using ECommerce.Domain.Constants;
using ECommerce.UseCases.Features.Stocks.Commands.AdjustStock;
using ECommerce.UseCases.Features.Stocks.Queries.GetPagedStocks;
using ECommerce.UseCases.Features.Stocks.Queries.GetPagedStockTransactions;
using ECommerce.UseCases.Features.Stocks.Queries.GetStockByProductId;
using ECommerce.UseCases.Features.Stocks.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

/// <summary>
/// API Controller responsible for managing inventory stocks and transaction logs.
/// Provides administrative endpoints for retrieving stock levels, transaction histories, and performing stock adjustments.
/// </summary>
[Authorize(Roles = $"{Roles.SuperAdmin},{Roles.Admin}")]
public class StocksController(IMediator mediator) : ApiControllerBase
{
    /// <summary>
    /// Get a paginated list of all stocks
    /// </summary>
    /// <param name="request">A query used to specify pagination, sorting, and stock filtering options</param>
    /// <param name="ct">A CancellationToken used to cancel the request</param>
    /// <returns>
    /// Returns a paginated list of stocks
    /// </returns>
    /// <response code="200">Stocks retrieved successfully</response>
    /// <response code="400">Invalid validation or bad request</response>
    /// <response code="401">Unauthorized user</response>
    /// <response code="403">Forbidden. User does not have the required role</response>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<StockResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<StockResponse>>>> PagedStocks(
        [FromQuery] GetPagedStocksQuery request,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(request, ct);
        if (result.IsFailure)
            return Problem(result);

        return FromPagedResult(result, request.PageNumber, request.PageSize, StockMessages.PagedStocksRetrievedSuccessfully);
    }

    /// <summary>
    /// Get a paginated list of stock transactions for a specific product
    /// </summary>
    /// <param name="productId">The unique identifier of the product</param>
    /// <param name="request">A query used to specify pagination and filtering for stock transactions</param>
    /// <param name="ct">A CancellationToken used to cancel the request</param>
    /// <returns>
    /// Returns a paginated list of stock transactions for the specified product
    /// </returns>
    /// <response code="200">Stock transactions retrieved successfully</response>
    /// <response code="400">Invalid validation or bad request</response>
    /// <response code="401">Unauthorized user</response>
    /// <response code="403">Forbidden. User does not have the required role</response>
    /// <response code="404">Stock or product was not found</response>
    [HttpGet("/api/v{version:apiVersion}/products/{productId:guid}/stock/transactions")]
    [ProducesResponseType(typeof(ApiResponse<IReadOnlyList<StockTransactionResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<IReadOnlyList<StockTransactionResponse>>>> PagedProductStockTransactions(
        [FromRoute] Guid productId,
        [FromQuery] GetPagedStockTransactionsQuery request,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(
            request with { ProductId = productId },
            ct);

        if (result.IsFailure)
            return Problem(result);

        return FromPagedResult(result, request.PageNumber, request.PageSize, StockMessages.TransactionsRetrievedSuccessfully);
    }

    /// <summary>
    /// Gets the stock details for a specific product by its Id
    /// </summary>
    /// <param name="productId">The unique identifier of the product</param>
    /// <param name="ct">A CancellationToken used to cancel the request</param>
    /// <returns>
    /// Returns the stock details if found, otherwise returns a 404 Not Found response
    /// </returns>
    /// <response code="200">Stock details retrieved successfully</response>
    /// <response code="400">Invalid validation or bad request</response>
    /// <response code="401">Unauthorized user</response>
    /// <response code="403">Forbidden. User does not have the required role</response>
    /// <response code="404">Stock for the specified product was not found</response>
    [HttpGet("/api/v{version:apiVersion}/products/{productId:guid}/stock")]
    [ProducesResponseType(typeof(ApiResponse<StockResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<StockResponse>>> GetStockByProductId(
        [FromRoute] Guid productId,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(new GetStockByProductIdQuery(productId), ct);
        if (result.IsFailure)
            return Problem(result);

        return Success(result.Value, StockMessages.StockRetrievedSuccessfully);
    }

    /// <summary>
    /// Adjusts the stock quantity for a specific product
    /// </summary>
    /// <param name="productId">The unique identifier of the product whose stock is being adjusted</param>
    /// <param name="request">The details required to adjust the stock quantity</param>
    /// <param name="ct">A CancellationToken used to cancel the request</param>
    /// <returns>
    /// Returns a success message if the stock quantity was updated successfully
    /// </returns>
    /// <response code="200">Stock quantity adjusted successfully</response>
    /// <response code="400">Invalid validation or bad request</response>
    /// <response code="401">Unauthorized user</response>
    /// <response code="403">Forbidden. User does not have the required role</response>
    /// <response code="404">Stock for the specified product was not found</response>
    [HttpPost("/api/v{version:apiVersion}/products/{productId:guid}/stock/adjustment")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AdjustStockByProductId(
        [FromRoute] Guid productId,
        [FromBody] AdjustStockRequest request,
        CancellationToken ct = default)
    {
        var result = await mediator.Send(
            new AdjustStockCommand(productId, request.NewQuantity, request.Notes, request.RowVersion),
            ct);

        if (result.IsFailure)
            return Problem(result);

        return Success(StockMessages.StockAdjustedSuccessfully);
    }
}