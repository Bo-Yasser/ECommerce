using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Common.Exceptions;
using ECommerce.UseCases.Features.Products.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.Products.Commands.DeleteProduct;

public sealed class DeleteProductCommandHandler(
    IRepository<Product> repository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteProductCommand, Result>
{
    public async Task<Result> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var existingProduct = await repository.FirstOrDefaultAsync(
            new ProductByIdSpecification(request.Id),
            cancellationToken);

        if (existingProduct is null)
            return Result.Failure(ProductErrors.NotFound);

        if (!existingProduct.RowVersion.SequenceEqual(request.RowVersion))
            return Result.Failure(ProductErrors.ConcurrencyConflict);

        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            repository.Delete(existingProduct);
            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return Result.Success();
        }
        catch (ConcurrencyConflictException)
        {
            return Result.Failure(ProductErrors.ConcurrencyConflict);
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }
}
