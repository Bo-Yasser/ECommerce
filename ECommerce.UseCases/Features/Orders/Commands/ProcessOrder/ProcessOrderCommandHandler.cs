using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Entities.OrderAggregate;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Common.Exceptions;
using ECommerce.UseCases.Features.Orders.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.Orders.Commands.ProcessOrder;

public sealed class ProcessOrderCommandHandler(
    IRepository<Order> repository,
    IUnitOfWork unitOfWork) : IRequestHandler<ProcessOrderCommand, Result>
{
    public async Task<Result> Handle(ProcessOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await repository.FirstOrDefaultAsync(
            new OrderByIdSpecification(request.OrderId),
            cancellationToken);

        if (order is null)
            return Result.Failure(OrderErrors.NotFound);

        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {

            if (!order.RowVersion.SequenceEqual(request.RowVersion))
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return Result.Failure(OrderErrors.ConcurrencyConflict);
            }

            var processResult = order.Process();
            if (processResult.IsFailure)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return Result.Failure(processResult.Error!);
            }

            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return Result.Success();
        }
        catch (ConcurrencyConflictException)
        {
            return Result.Failure(OrderErrors.ConcurrencyConflict);
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}
