using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Features.ProductTypes.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.ProductTypes.Commands.CreateType;

public sealed class CreateTypeCommandHandler(
    IRepository<ProductType> repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateTypeCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateTypeCommand request, CancellationToken cancellationToken)
    {
        var existingName = await repository.AnyAsync(
            new TypeByNameSpecification(request.Name),
            cancellationToken);

        if (existingName)
            return Result<Guid>.Failure(TypeErrors.AlreadyExists);

        var id = Guid.NewGuid();
        var typeResult = ProductType.Create(id, request.Name);

        if (typeResult.IsFailure)
            return Result<Guid>.Failure(typeResult.Error!);

        repository.Add(typeResult.Value);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(id);
    }
}
