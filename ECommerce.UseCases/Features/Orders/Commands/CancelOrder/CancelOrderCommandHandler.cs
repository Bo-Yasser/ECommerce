using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Entities.OrderAggregate;
using ECommerce.Domain.Entities.StockAggregate;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Common.Exceptions;
using ECommerce.UseCases.Common.Interfaces;
using ECommerce.UseCases.Features.Orders.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.Orders.Commands.CancelOrder;

public sealed class CancelOrderCommandHandler(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService,
    IRepository<Order> ordersRepository,
    IRepository<Stock> stocksRepository) : IRequestHandler<CancelOrderCommand, Result>
{
    public async Task<Result> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        // Get the authenticated user id
        var userId = currentUserService.UserId;
        if (userId is null)
            return Result.Failure(OrderErrors.TokenMissing);

        // Start a transaction to keep order cancellation and stock restoration atomic
        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            // Get the order with its items, ensuring it belongs to the current user
            var order = await ordersRepository.FirstOrDefaultAsync(
                new OrderWithItemsByIdSpecification(request.OrderId, userId.Value),
                cancellationToken);

            if (order is null)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return Result.Failure(OrderErrors.NotFound);
            }

            // Ensure the client is working with the latest order version
            if (!request.RowVersion.SequenceEqual(order.RowVersion))
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return Result.Failure(OrderErrors.ConcurrencyConflict);
            }

            // Get all stocks related to the order items
            var stocks = await stocksRepository.ListAsync(
                new StocksListByOrderItemSpecification(order.Items),
                cancellationToken);

            if (stocks.Count != order.Items.Count)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return Result.Failure(OrderErrors.StockNotFound);
            }

            var stocksDictionary = stocks.ToDictionary(s => s.ProductId);

            // Restore the stock quantities for all order items
            foreach (var item in order.Items)
            {
                var stock = stocksDictionary[item.ProductId];

                var restoreResult = stock.Restore(
                    amount: item.Quantity,
                    type: StockTransactionType.OrderCancellation,
                    notes: "System Auto-Restoring for Order Cancellation",
                    referenceId: order.Id);

                if (restoreResult.IsFailure)
                {
                    await unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return Result.Failure(restoreResult.Error!);
                }
            }

            // Change the order status to Cancelled
            var cancelResult = order.Cancel();

            if (cancelResult.IsFailure)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return Result.Failure(cancelResult.Error!);
            }

            // Save all changes and commit the transaction
            await unitOfWork.CommitTransactionAsync(cancellationToken);

            return Result.Success();
        }
        catch (ConcurrencyConflictException)
        {
            // Translate database concurrency conflicts into an application error
            return Result.Failure(OrderErrors.ConcurrencyConflict);
        }
        catch
        {
            // Roll back the transaction for any unexpected exception
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}