using System.Linq.Expressions;
using ECommerce.UseCases.Specifications;

namespace ECommerce.UseCases.Specifications.Orders;
public interface IOrderSpecificationBuilder<T> : ISpecificationBuilder<T>
{
    ISpecificationBuilder<T> ThenBy(Expression<Func<T, object?>> orderExpression);
    ISpecificationBuilder<T> ThenByDescending(Expression<Func<T, object?>> orderExpression);
}

public interface IOrderSpecificationBuilder<T, TResult> : ISpecificationBuilder<T, TResult>
{
    ISpecificationBuilder<T, TResult> ThenBy(Expression<Func<T, object?>> orderExpression);
    ISpecificationBuilder<T, TResult> ThenByDescending(Expression<Func<T, object?>> orderExpression);
}