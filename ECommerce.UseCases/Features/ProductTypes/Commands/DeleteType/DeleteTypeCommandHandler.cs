using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Features.ProductTypes.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.ProductTypes.Commands.DeleteType;

public sealed class DeleteTypeCommandHandler(
    IRepository<ProductType> repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteTypeCommand, Result>
{
    public async Task<Result> Handle(DeleteTypeCommand request, CancellationToken cancellationToken)
    {
        var existingType = await repository.FirstOrDefaultAsync(
            new TypeByIdSpecification(request.Id),
            cancellationToken);

        if (existingType is null)
            return Result.Failure(TypeErrors.NotFound);

        repository.Delete(existingType);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
