using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Common.Exceptions;
using ECommerce.UseCases.Features.Products.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.Products.Commands.UpdateProduct;
public sealed class UpdateProductCommandHandler(
    IRepository<Product> productsRepository,
    IReadRepository<ProductBrand> brandsRepository,
    IReadRepository<ProductType> typesRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateProductCommand, Result>
{
    public async Task<Result> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        // check if the product exist
        var exisitingProduct = await productsRepository.FirstOrDefaultAsync(
            new ProductByIdSpecification(request.Id),
            cancellationToken);
        if (exisitingProduct is null)
            return Result.Failure(ProductErrors.NotFound);

        if (!exisitingProduct.RowVersion.SequenceEqual(request.RowVersion))
            return Result.Failure(ProductErrors.ConcurrencyConflict);

        // check if the brand exist
        var brandExists = await brandsRepository.AnyAsync(
            new ProductBrandSpecification(request.ProductBrandId),
            cancellationToken);
        if (!brandExists)
            return Result.Failure(ProductErrors.ProductBrandNotFound);

        // check if the type exist
        var typeExists = await typesRepository.AnyAsync(
            new ProductTypeSpecification(request.ProductTypeId),
            cancellationToken);
        if (!typeExists)
            return Result.Failure(ProductErrors.ProductTypeNotFound);

        // check if the product sku exist
        var existingProductSku = await productsRepository.AnyAsync(
            new ProductBySkuSpecificaiton(request.Sku, request.Id),
            cancellationToken);
        if (existingProductSku)
            return Result.Failure(ProductErrors.SkuAlreadyExists);

        // check if the product name exist
        var exisitingProductName = await productsRepository.AnyAsync(
            new ProductByNameSpecification(request.Name, request.Id),
            cancellationToken);
        if (exisitingProductName)
            return Result.Failure(ProductErrors.NameAlreadyExists);

        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            var productResult = exisitingProduct.Update(
                request.Sku,
                request.Name,
                request.Description,
                request.PictureUrl,
                request.Price,
                request.ProductBrandId,
                request.ProductTypeId);

            if (productResult.IsFailure)
            {
                await unitOfWork.RollbackTransactionAsync(cancellationToken);
                return Result.Failure(productResult.Error!);
            }

            await unitOfWork.CommitTransactionAsync(cancellationToken);
            return Result.Success();

        }
        catch(ConcurrencyConflictException)
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

