using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Features.ProductBrands.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.ProductBrands.Commands.DeleteBrand;

public sealed class DeleteBrandCommandHandler(
    IRepository<ProductBrand> repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteBrandCommand, Result>
{
    public async Task<Result> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
    {
        var existingBrand = await repository.FirstOrDefaultAsync(
            new BrandByIdSpecification(request.Id),
            cancellationToken);

        if (existingBrand is null)
            return Result.Failure(BrandErrors.AlreadyExists);

        repository.Delete(existingBrand);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
