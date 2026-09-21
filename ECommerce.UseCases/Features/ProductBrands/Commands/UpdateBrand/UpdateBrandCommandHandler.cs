using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Features.ProductBrands.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.ProductBrands.Commands.UpdateBrand;

public sealed class UpdateBrandCommandHandler(
    IRepository<ProductBrand> repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateBrandCommand, Result>
{
    public async Task<Result> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
    {
        var existingBrand = await repository.FirstOrDefaultAsync(
            new BrandByNameSpecification(request.Name, request.Id),
            cancellationToken);

        if (existingBrand is null)
            return Result.Failure(BrandErrors.AlreadyExists);

        var updateResult = existingBrand.Update(request.Name);

        if (updateResult.IsFailure)
            return Result.Failure(updateResult.Error!);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}