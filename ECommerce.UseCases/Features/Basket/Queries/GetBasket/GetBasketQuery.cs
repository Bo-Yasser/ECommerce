using ECommerce.Domain.Common;
using ECommerce.UseCases.Features.Basket.Responses;
using MediatR;

namespace ECommerce.UseCases.Features.Basket.Queries.GetBasket;

public record GetBasketQuery(Guid BuyerId) : IRequest<Result<GetBasketResponse>>;
