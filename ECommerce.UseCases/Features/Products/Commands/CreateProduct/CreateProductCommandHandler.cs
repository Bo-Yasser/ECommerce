using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.StockAggregate;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Features.Products.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.Products.Commands.CreateProduct;

public sealed class CreateProductCommandHandler(
    IRepository<Product> productsRepository,
    IRepository<Stock> stocksRepository,
    IReadRepository<ProductBrand> brandsRepository,
    IReadRepository<ProductType> typesRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateProductCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var brandExists = await brandsRepository.AnyAsync(
            new ProductBrandSpecification(request.ProductBrandId),
            cancellationToken);
        if (!brandExists)
            return Result<Guid>.Failure(ProductErrors.ProductBrandNotFound);

        var typeExists = await typesRepository.AnyAsync(
            new ProductTypeSpecification(request.ProductTypeId),
            cancellationToken);
        if (!typeExists)
            return Result<Guid>.Failure(ProductErrors.ProductTypeNotFound);

        var existingProductName = await productsRepository.AnyAsync(
            new ProductByNameSpecification(request.Name),
            cancellationToken);
        if (existingProductName)
            return Result<Guid>.Failure(ProductErrors.AlreadyExists);


        var productId = Guid.NewGuid();

        var productResult = Product.Create(
            productId,
            request.Sku,
            request.Name,
            request.Description,
            request.PictureUrl,
            request.Price,
            request.ProductBrandId,
            request.ProductTypeId);

        if (productResult.IsFailure)
            return Result<Guid>.Failure(productResult.Error!);

        var stockResult = Stock.Create(
            Guid.NewGuid(),
            productId,
            0);
        if (stockResult.IsFailure)
            return Result<Guid>.Failure(stockResult.Error!);

        productsRepository.Add(productResult.Value);
        stocksRepository.Add(stockResult.Value);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(productId);
    }
}
