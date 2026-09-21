using ECommerce.Domain.Specifications;
using System.Linq.Expressions;

namespace ECommerce.UseCases.Specifications.Orders;
internal sealed class OrderSpecificationBuilder<T> : SpecificationBuilder<T>, IOrderSpecificationBuilder<T>
{
    internal OrderSpecificationBuilder(Specification<T> specification) : base(specification)
    {
    }

    public IOrderSpecificationBuilder<T> ThenBy(Expression<Func<T, object?>> orderExpression)
    {
        _specification.AddOrder(new OrderExpressionInfo<T>(orderExpression, OrderType.ThenBy));
        return this;
    }

    public IOrderSpecificationBuilder<T> ThenByDescending(Expression<Func<T, object?>> orderExpression)
    {
        _specification.AddOrder(new OrderExpressionInfo<T>(orderExpression, OrderType.ThenByDescending));
        return this;
    }
}


internal sealed class OrderSpecificationBuilder<T, TResult> 
    : SpecificationBuilder<T, TResult>, IOrderSpecificationBuilder<T, TResult>
{
    internal OrderSpecificationBuilder(Specification<T, TResult> specification) : base(specification)
    {
    }

    public IOrderSpecificationBuilder<T, TResult> ThenBy(Expression<Func<T, object?>> orderExpression)
    {
        _specification.AddOrder(new OrderExpressionInfo<T>(orderExpression, OrderType.ThenBy));
        return this;
    }

    public IOrderSpecificationBuilder<T, TResult> ThenByDescending(Expression<Func<T, object?>> orderExpression)
    {
        _specification.AddOrder(new OrderExpressionInfo<T>(orderExpression, OrderType.ThenByDescending));
        return this;
    }
}
