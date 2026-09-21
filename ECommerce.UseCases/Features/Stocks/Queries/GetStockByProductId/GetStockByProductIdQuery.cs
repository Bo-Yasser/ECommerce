using ECommerce.Domain.Common;
using ECommerce.UseCases.Features.Stocks.Responses;
using MediatR;

namespace ECommerce.UseCases.Features.Stocks.Queries.GetStockByProductId;

public sealed record GetStockByProductIdQuery(Guid ProductId) : IRequest<Result<StockResponse>>;
