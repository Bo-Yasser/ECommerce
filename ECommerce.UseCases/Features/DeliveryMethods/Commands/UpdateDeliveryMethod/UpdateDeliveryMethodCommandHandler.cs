using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Features.DeliveryMethods.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.DeliveryMethods.Commands.UpdateDeliveryMethod;

public sealed class UpdateDeliveryMethodCommandHandler(
    IRepository<DeliveryMethod> repository,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateDeliveryMethodCommand, Result>
{
    public async Task<Result> Handle(UpdateDeliveryMethodCommand request, CancellationToken cancellationToken)
    {
        var deliveryMethod = await repository.FirstOrDefaultAsync(
            new DeliveryMethodByIdSpecification(request.Id),
            cancellationToken);
        if (deliveryMethod is null)
            return Result.Failure(DeliveryMethodErrors.NotFound);

        var nameExists = await repository.AnyAsync(
            new DeliveryMethodByNameSpecification(request.Name, request.Id),
            cancellationToken);
        if (nameExists)
            return Result.Failure(DeliveryMethodErrors.NameAlreadyExists);

        var updateResult = deliveryMethod.Update(
            name: request.Name,
            price: request.Price,
            estimatedDeliveryTime: request.EstimatedDeliveryTime,
            description: request.Description,
            isAvailable: request.IsAvailable,
            displayOrder: request.DisplayOrder);

        if (updateResult.IsFailure)
            return Result.Failure(updateResult.Error!);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
