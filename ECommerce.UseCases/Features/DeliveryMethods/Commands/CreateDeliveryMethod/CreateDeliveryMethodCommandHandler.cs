using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Features.DeliveryMethods.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.DeliveryMethods.Commands.CreateDeliveryMethod;

public sealed class CreateDeliveryMethodCommandHandler(
    IRepository<DeliveryMethod> repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateDeliveryMethodCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateDeliveryMethodCommand request, CancellationToken cancellationToken)
    {
        var nameExists = await repository.AnyAsync(
            new DeliveryMethodByNameSpecification(request.Name),
            cancellationToken);
        if (nameExists)
            return Result<Guid>.Failure(DeliveryMethodErrors.NameAlreadyExists);

        var id = Guid.NewGuid();

        var createResult = DeliveryMethod.Create(
            id,
            request.Name,
            request.Price,
            request.EstimatedDeliveryTime,
            request.Description,
            request.IsAvailable,
            request.DisplayOrder);

        if (createResult.IsFailure)
            return Result<Guid>.Failure(createResult.Error!);

        repository.Add(createResult.Value);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(id);
    }
}
