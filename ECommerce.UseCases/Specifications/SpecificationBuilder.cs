using ECommerce.Domain.Specifications;
using ECommerce.UseCases.Specifications.Includes;
using ECommerce.UseCases.Specifications.Orders;
using System.Linq.Expressions;

namespace ECommerce.UseCases.Specifications;

public class SpecificationBuilder<T> : ISpecificationBuilder<T>
{
    protected readonly Specification<T> _specification;
    internal SpecificationBuilder(Specification<T> specification)
    {
        _specification = specification;
    }

    public ISpecificationBuilder<T> Where(Expression<Func<T, bool>> predicate)
    {
        _specification.AddWhere(predicate);
        return this;
    }
    public IIncludableSpecificationBuilder<T, TProperty> Include<TProperty>(Expression<Func<T, TProperty>> navigation)
    {
        var parent = _specification.AddInclude(navigation);
        return new IncludableSpecificationBuilder<T, TProperty>(_specification, parent);
    }

    public IIncludableCollectionSpecificationBuilder<T, TElement> Include<TElement>(Expression<Func<T, ICollection<TElement>>> navigation)
    {
        var parent = _specification.AddInclude(navigation);
        return new IncludableCollectionSpecificationBuilder<T, TElement>(_specification, parent);
    }
    public IOrderSpecificationBuilder<T> OrderBy(Expression<Func<T, object?>> orderExpression)
    {
        _specification.AddOrder(new OrderExpressionInfo<T>(orderExpression, OrderType.OrderBy));
        return new OrderSpecificationBuilder<T>(_specification);
    }

    public IOrderSpecificationBuilder<T> OrderByDescending(Expression<Func<T, object?>> orderExpression)
    {
        _specification.AddOrder(new OrderExpressionInfo<T>(orderExpression, OrderType.OrderByDescending));
        return new OrderSpecificationBuilder<T>(_specification);
    }
    public ISpecificationBuilder<T> Skip(int skip)
    {
        _specification.SetSkip(skip);
        return this;
    }

    public ISpecificationBuilder<T> Take(int take)
    {
        _specification.SetTake(take);
        return this;
    }
    public ISpecificationBuilder<T> AsNoTracking()
    {
        _specification.SetNoTracking();
        return this;
    }
    public ISpecificationBuilder<T> AsTracking()
    {
        _specification.SetTracking();
        return this;
    }
}

public class SpecificationBuilder<T, TResult> : ISpecificationBuilder<T, TResult>
{
    protected readonly Specification<T, TResult> _specification;
    private readonly SpecificationBuilder<T> _builder;
    internal SpecificationBuilder(Specification<T, TResult> specification)
    {
        _specification = specification;
        _builder = new SpecificationBuilder<T>(specification);
    }

    public ISpecificationBuilder<T, TResult> Where(Expression<Func<T, bool>> predicate)
    {
        _builder.Where(predicate);
        return this;
    }
    public IOrderSpecificationBuilder<T, TResult> OrderBy(Expression<Func<T, object?>> orderExpression)
    {
        _builder.OrderBy(orderExpression);
        return new OrderSpecificationBuilder<T, TResult>(_specification);
    }

    public IOrderSpecificationBuilder<T, TResult> OrderByDescending(Expression<Func<T, object?>> orderExpression)
    {
        _builder.OrderByDescending(orderExpression);
        return new OrderSpecificationBuilder<T, TResult>(_specification);
    }
    public ISpecificationBuilder<T, TResult> Skip(int skip)
    {
        _builder.Skip(skip);
        return this;
    }

    public ISpecificationBuilder<T, TResult> Take(int take)
    {
        _builder.Take(take);
        return this;
    }

    public ISpecificationBuilder<T, TResult> AsNoTracking()
    {
        _builder.AsNoTracking();
        return this;
    }

    public ISpecificationBuilder<T, TResult> AsTracking()
    {
        _builder.AsTracking();
        return this;
    }

    public ISpecificationBuilder<T, TResult> Select(Expression<Func<T, TResult>> selector)
    {
        _specification.SetSelector(selector);
        return this;
    }

    public ISpecificationBuilder<T, TResult> SelectMany(Expression<Func<T, IEnumerable<TResult>>> selector)
    {
        _specification.SetSelectorMany(selector);
        return this;
    }

}



