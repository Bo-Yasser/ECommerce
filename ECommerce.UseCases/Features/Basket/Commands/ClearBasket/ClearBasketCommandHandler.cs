using ECommerce.Domain.Common;
using ECommerce.Domain.Repositories;
using ECommerce.UseCases.Common.Interfaces;
using ECommerce.UseCases.Features.Basket.Responses;
using MediatR;

namespace ECommerce.UseCases.Features.Basket.Commands.ClearBasket;

public sealed class ClearBasketCommandHandler(
    IBasketStore basketStore,
    ICurrentUserService currentUserService)
    : IRequestHandler<ClearBasketCommand, Result<GetBasketResponse>>
{
    public async Task<Result<GetBasketResponse>> Handle(ClearBasketCommand request, CancellationToken cancellationToken)
    {
        var isGuest = !currentUserService.IsAuthenticated;
        var buyerId = currentUserService.BuyerId;

        if(buyerId is null)
        {
            var newGuestId = Guid.NewGuid();
            var emptyBasket = Domain.Entities.Basket.CreateEmpty(newGuestId);

            return Result<GetBasketResponse>.Success(GetBasketResponse.From(emptyBasket.Value, isGuest));
        }

        await basketStore.DeleteAsync(buyerId.Value, cancellationToken);

        var clearedBasket = Domain.Entities.Basket.CreateEmpty(buyerId.Value);
        return Result<GetBasketResponse>.Success(GetBasketResponse.From(clearedBasket.Value, isGuest));
    }
}
