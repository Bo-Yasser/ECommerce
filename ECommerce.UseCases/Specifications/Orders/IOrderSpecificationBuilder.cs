using System.Linq.Expressions;

namespace ECommerce.UseCases.Specifications.Orders;
public interface IOrderSpecificationBuilder<T> : ISpecificationBuilder<T>
{
    IOrderSpecificationBuilder<T> ThenBy(Expression<Func<T, object?>> orderExpression);
    IOrderSpecificationBuilder<T> ThenByDescending(Expression<Func<T, object?>> orderExpression);
}

public interface IOrderSpecificationBuilder<T, TResult> : ISpecificationBuilder<T, TResult>
{
    IOrderSpecificationBuilder<T, TResult> ThenBy(Expression<Func<T, object?>> orderExpression);
    IOrderSpecificationBuilder<T, TResult> ThenByDescending(Expression<Func<T, object?>> orderExpression);
}