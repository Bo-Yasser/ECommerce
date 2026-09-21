using ECommerce.Domain.Common;
using ECommerce.UseCases.Features.Basket.Responses;
using MediatR;

namespace ECommerce.UseCases.Features.Basket.Queries.GetBasket;

public sealed record GetBasketQuery() : IRequest<Result<BasketResponse>>;
