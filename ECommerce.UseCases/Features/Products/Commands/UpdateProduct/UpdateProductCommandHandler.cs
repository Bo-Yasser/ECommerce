using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
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
        var exisitingProduct = await productsRepository.FirstOrDefaultAsync(
            new ProductByIdSpecification(request.Id),
            cancellationToken);

        if (exisitingProduct is null)
            return Result.Failure(ProductErrors.NotFound);

        var brandExists = await brandsRepository.AnyAsync(
            new ProductBrandSpecification(request.ProductBrandId),
            cancellationToken);
        if (!brandExists)
            return Result.Failure(ProductErrors.ProductBrandNotFound);

        var typeExists = await typesRepository.AnyAsync(
            new ProductTypeSpecification(request.ProductTypeId),
            cancellationToken);
        if (!typeExists)
            return Result.Failure(ProductErrors.ProductTypeNotFound);

        var exisitingProductName = await productsRepository.AnyAsync(
            new ProductByNameSpecification(request.Name, request.Id),
            cancellationToken);
        if (exisitingProductName)
            return Result.Failure(ProductErrors.AlreadyExists);

        var productResult = exisitingProduct.Update(
            request.Sku,
            request.Name,
            request.Description,
            request.PictureUrl,
            request.Price,
            request.ProductBrandId,
            request.ProductTypeId);

        if (productResult.IsFailure)
            return Result.Failure(productResult.Error!);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

