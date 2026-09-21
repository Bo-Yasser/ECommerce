using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
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


        repository.Delete(deliveryMethod);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}