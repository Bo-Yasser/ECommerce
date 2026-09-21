using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Features.ProductBrands.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.ProductBrands.Commands.CreateBrand;

public sealed class CreateBrandCommandHandler(
    IRepository<ProductBrand> repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateBrandCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
    {
        var existingName = await repository.AnyAsync(
            new BrandByNameSpecification(request.Name),
            cancellationToken);

        if (existingName)
            return Result<Guid>.Failure(BrandErrors.AlreadyExists);

        var id = Guid.NewGuid();
        var brandResult = ProductBrand.Create(id, request.Name);

        if (brandResult.IsFailure)
            return Result<Guid>.Failure(brandResult.Error!);

        repository.Add(brandResult.Value);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(id);
    }
}
