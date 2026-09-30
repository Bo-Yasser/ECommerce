using ECommerce.Domain.Common;
using ECommerce.Domain.Common.Errors;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Entities.OrderAggregate;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Common.Exceptions;
using ECommerce.UseCases.Common.Interfaces;
using ECommerce.UseCases.Features.Orders.Responses;
using ECommerce.UseCases.Features.Orders.Specifications;
using MediatR;

namespace ECommerce.UseCases.Features.Orders.Commands.CreateOrder;

public sealed class CreateOrderCommandHandler(
    IUnitOfWork unitOfWork,
    IBasketStore basketStore,
    ICurrentUserService currentUserService,
    IReadRepository<Product> productsRepository,
    IRepository<DeliveryMethod> deliveryMethodsRepository,
    IReadRepository<UserAddress> userAddressesRepository,
    IRepository<Order> ordersRepository)
    : IRequestHandler<CreateOrderCommand, Result<OrderResponse>>
{
    public async Task<Result<OrderResponse>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        // get authenticated user id
        var userId = currentUserService.UserId;
        if (userId is null)
            return Result<OrderResponse>.Failure(OrderErrors.TokenMissing);

        // get basket for the authenticated user
        var basket = await basketStore.GetAsync(userId.Value, cancellationToken);
        
        // check if basket exist or basket is empty
        if(basket is null)
            return Result<OrderResponse>.Failure(OrderErrors.BasketNotFound);
        if (basket.Items.Count == 0)
            return Result<OrderResponse>.Failure(OrderErrors.BasketEmpty);

        // get related products
        var productsList = await productsRepository.ListAsync(
            new ProductsListWithStockByBasketItemSpecification(basket.Items),
            cancellationToken);

        // distinct the products
        var productsDictionary = productsList.ToDictionary(p => p.Id);

        // check for missing/invalid product ids
        if(productsDictionary.Count != basket.Items.Select(i => i.ProductId).Distinct().Count())
            return Result<OrderResponse>.Failure(OrderErrors.ProductItemInvalidId);

        // get the delivery method
        var deliveryMethod = await deliveryMethodsRepository.FirstOrDefaultAsync(
            new DeliveryMethodByIdSpecification(request.DeliveryMethodId),
            cancellationToken);
        if(deliveryMethod is null)
            return Result<OrderResponse>.Failure(OrderErrors.OrderDeliveryMethodInvalidId);

        // get the user address
        var userAddress = await userAddressesRepository.FirstOrDefaultAsync(
            new UserAddressByIdSpecification(request.ShippingAddressId, userId.Value),
            cancellationToken);
        if (userAddress is null)
            return Result<OrderResponse>.Failure(OrderErrors.ShippingAddressNotOwned);

        // create order 
        var orderResult = Order.Create(
            id: Guid.NewGuid(),
            userId: userId.Value,
            deliveryMethod,
            userAddress);

        if (orderResult.IsFailure)
            return Result<OrderResponse>.Failure(orderResult.Error!);

        var order = orderResult.Value;
        

        await unitOfWork.BeginTransactionAsync(cancellationToken);
        try
        {
            // loop on basket items
            foreach(var basketItem in basket.Items)
            {
                // get the related product
                var product = productsDictionary[basketItem.ProductId];

                // get the product stock
                var stock = product.Stock;

                if (stock is null)
                {
                    await unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return Result<OrderResponse>.Failure(OrderErrors.StockNotFound);
                }

                // deduct the product stock
                var deductResult = stock.Deduct(
                    amount: basketItem.Quantity,
                    type: StockTransactionType.OrderDeduction,
                    notes: "System Auto-Deduction for Order Placement",
                    referenceId: order.Id);
                
                if (deductResult.IsFailure)
                {
                    await unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return Result<OrderResponse>.Failure(deductResult.Error!);
                }

                // convert related product to ProductItemOrdered snapshot
                var itemOrderedResult = ProductItemOrdered.Create(
                    sku: product.Sku,
                    productName: product.Name,
                    pictureUrl: product.PictureUrl,
                    unitPrice: product.Price);

                if (itemOrderedResult.IsFailure)
                {
                    await unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return Result<OrderResponse>.Failure(itemOrderedResult.Error!);
                }

                // add order item
                var addOrderItemResult = order.AddItem(
                    productId: basketItem.ProductId,
                    itemOrdered: itemOrderedResult.Value,
                    quantity: basketItem.Quantity);

                if (addOrderItemResult.IsFailure)
                {
                    await unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return Result<OrderResponse>.Failure(addOrderItemResult.Error!);
                }
            }

            // add order to the database
            ordersRepository.Add(order);

            // commit the changes
            await unitOfWork.CommitTransactionAsync(cancellationToken);
        }
        catch (ConcurrencyConflictException)
        {
            return Result<OrderResponse>.Failure(OrderErrors.ConcurrencyConflict);
        }
        catch
        {
            await unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
        // delete the basket from the cache
        await basketStore.DeleteAsync(userId.Value, cancellationToken);

        return Result<OrderResponse>.Success(OrderResponse.From(order));

    }
}
