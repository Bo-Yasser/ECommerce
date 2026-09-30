using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Entities.StockAggregate;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Common.Exceptions;
using ECommerce.UseCases.Common.Interfaces;
using ECommerce.UseCases.Features.Stocks.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.Stocks.Commands.AdjustStock;

public sealed class AdjustStockCommandHandler(
    IRepository<Stock> stockRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUserService)
    : IRequestHandler<AdjustStockCommand, Result>
{
    public async Task<Result> Handle(AdjustStockCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (userId is null)
            return Result.Failure(AuthErrors.TokenMissing);

        await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var stock = await stockRepository.FirstOrDefaultAsync(
                new StockByProductIdSpecification(request.ProductId),
                cancellationToken);

            if (stock is null)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return Result.Failure(StockErrors.NotFound);
            }

            if (!stock.RowVersion.SequenceEqual(request.RowVersion))
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return Result.Failure(StockErrors.ConcurrencyConflict);
            }

            var operationResult = stock.AdjustQuantity(request.NewQuantity, request.Notes, userId);
            if (operationResult.IsFailure)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return Result.Failure(operationResult.Error!);
            }

            await unitOfWork.CommitTransactionAsync(cancellationToken);

            return Result.Success();
        }
        catch(ConcurrencyConflictException)
        {
            return Result.Failure(StockErrors.ConcurrencyConflict);
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}
