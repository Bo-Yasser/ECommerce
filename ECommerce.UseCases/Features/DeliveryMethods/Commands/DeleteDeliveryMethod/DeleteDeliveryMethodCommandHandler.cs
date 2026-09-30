using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Common.Exceptions;
using ECommerce.UseCases.Features.DeliveryMethods.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.DeliveryMethods.Commands.DeleteDeliveryMethod;

public sealed class DeleteDeliveryMethodCommandHandler(
    IRepository<DeliveryMethod> repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteDeliveryMethodCommand, Result>
{
    public async Task<Result> Handle(DeleteDeliveryMethodCommand request, CancellationToken cancellationToken)
    {
        var deliveryMethod = await repository.FirstOrDefaultAsync(
            new DeliveryMethodByIdSpecification(request.Id),
            cancellationToken);

        if (deliveryMethod is null)
            return Result.Failure(DeliveryMethodErrors.NotFound);

        if (!deliveryMethod.RowVersion.SequenceEqual(request.RowVersion))
            return Result.Failure(DeliveryMethodErrors.ConcurrencyConflict);

        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            repository.Delete(deliveryMethod);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return Result.Success();
        }
        catch (ConcurrencyConflictException)
        {
            return Result.Failure(DeliveryMethodErrors.ConcurrencyConflict);
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}