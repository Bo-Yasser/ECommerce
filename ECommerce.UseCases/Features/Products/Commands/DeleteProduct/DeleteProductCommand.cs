using ECommerce.Domain.Common;
using MediatR;

namespace ECommerce.UseCases.Features.Products.Commands.DeleteProduct;

public sealed record DeleteProductCommand(Guid Id) : IRequest<Result>;
