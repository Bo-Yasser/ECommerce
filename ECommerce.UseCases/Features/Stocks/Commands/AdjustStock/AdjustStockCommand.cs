using ECommerce.Domain.Common;
using MediatR;

namespace ECommerce.UseCases.Features.Stocks.Commands.AdjustStock;

public sealed record AdjustStockCommand(
    Guid ProductId,
    int NewQuantity,
    string? Notes,
    byte[] RowVersion) : IRequest<Result>;
