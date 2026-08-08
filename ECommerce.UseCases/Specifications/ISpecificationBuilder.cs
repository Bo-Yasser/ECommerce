using ECommerce.UseCases.Specifications.Includes;
using ECommerce.UseCases.Specifications.Orders;
using System.Linq.Expressions;

namespace ECommerce.UseCases.Specifications;

public interface ISpecificationBuilder<T>
{
    ISpecificationBuilder<T> Where(Expression<Func<T, bool>> predicate);

    // Includes & ThenIncludes
    IIncludableSpecificationBuilder<T, TProperty> Include<TProperty>(Expression<Func<T, TProperty>> navigation);
    IIncludableCollectionSpecificationBuilder<T, TElement> Include<TElement>(Expression<Func<T, ICollection<TElement>>> navigation);
    // Ordering
    IOrderSpecificationBuilder<T> OrderBy(Expression<Func<T, object?>> orderExpression);
    IOrderSpecificationBuilder<T> OrderByDescending(Expression<Func<T, object?>> orderExpression);
    ISpecificationBuilder<T> Skip(int skip);
    ISpecificationBuilder<T> Take(int take);
    ISpecificationBuilder<T> AsTracking();
    ISpecificationBuilder<T> AsNoTracking();
}


public interface ISpecificationBuilder<T, TResult>
{
    ISpecificationBuilder<T, TResult> Where(Expression<Func<T, bool>> predicate);
    // Ordering
    IOrderSpecificationBuilder<T, TResult> OrderBy(Expression<Func<T, object?>> orderExpression);
    IOrderSpecificationBuilder<T, TResult> OrderByDescending(Expression<Func<T, object?>> orderExpression);
    ISpecificationBuilder<T, TResult> Skip(int skip);
    ISpecificationBuilder<T, TResult> Take(int take);
    ISpecificationBuilder<T, TResult> AsTracking();
    ISpecificationBuilder<T, TResult> AsNoTracking();
    ISpecificationBuilder<T, TResult> Select(Expression<Func<T, TResult>> selector);
    ISpecificationBuilder<T, TResult> SelectMany(Expression<Func<T, IEnumerable<TResult>>> selector);
}