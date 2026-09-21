using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Features.ProductTypes.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.ProductTypes.Commands.UpdateType;

public sealed class UpdateTypeCommandHandler(
    IRepository<ProductType> repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateTypeCommand, Result>
{
    public async Task<Result> Handle(UpdateTypeCommand request, CancellationToken cancellationToken)
    {
        var existingType = await repository.FirstOrDefaultAsync(
            new TypeByNameSpecification(request.Name, request.Id),
            cancellationToken);

        if (existingType is null)
            return Result.Failure(TypeErrors.AlreadyExists);

        var updateResult = existingType.Update(request.Name);

        if (updateResult.IsFailure)
            return Result<Guid>.Failure(updateResult.Error!);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}