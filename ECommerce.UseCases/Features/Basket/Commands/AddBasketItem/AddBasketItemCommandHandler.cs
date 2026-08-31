using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Features.Basket.Responses;
using ECommerce.UseCases.Features.Basket.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.Basket.Commands.AddBasketItem;

public sealed class AddBasketItemCommandHandler(
    IBasketStore basketStore,
    IReadRepository<Product> productRepository)
    : IRequestHandler<AddBasketItemCommand, Result<GetBasketResponse>>
{
    public async Task<Result<GetBasketResponse>> Handle(AddBasketItemCommand request, CancellationToken cancellationToken)
    {
        var product = await productRepository.FirstOrDefaultAsync(
            new ProductForBasketSpecification(request.ProductId),
            cancellationToken);
        if (product is null)
            return Result<GetBasketResponse>.Failure(ProductErrors.NotFound);


        var basket = await basketStore.GetOrCreateAsync(request.BuyerId, cancellationToken);

        var addResult = basket.AddItem(
            productId: product.Id,
            productName: product.Name,
            pictureUrl: product.PictureUrl,
            unitPrice: product.Price,
            quantity: request.Quantity);

        if (addResult.IsFailure)
            return Result<GetBasketResponse>.Failure(addResult.Error!);

        await basketStore.SaveAsync(basket, cancellationToken);
        return Result<GetBasketResponse>.Success(GetBasketResponse.From(basket));
    }
}
