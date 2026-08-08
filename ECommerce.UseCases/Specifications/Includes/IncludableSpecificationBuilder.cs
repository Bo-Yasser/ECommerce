using System.Linq.Expressions;
using ECommerce.UseCases.Specifications;
using ECommerce.Domain.Specifications;

namespace ECommerce.UseCases.Specifications.Includes;
internal sealed class IncludableSpecificationBuilder<T, TProperty> : SpecificationBuilder<T>, IIncludableSpecificationBuilder<T, TProperty>
{
    private readonly LambdaExpression _parent;
    internal IncludableSpecificationBuilder(Specification<T> specification, LambdaExpression parent)
        : base(specification)
    {
        _parent = parent;
    }

    public IIncludableSpecificationBuilder<T, TNext> ThenInclude<TNext>(Expression<Func<TProperty, TNext>> navigation)
    {
        _specification.AddThenInclude(navigation, _parent);
        return new IncludableSpecificationBuilder<T, TNext>(_specification, navigation);
    }
}

internal sealed class IncludableCollectionSpecificationBuilder<T, TElement> : SpecificationBuilder<T>, IIncludableCollectionSpecificationBuilder<T, TElement>
{
    private readonly LambdaExpression _parent;
    internal IncludableCollectionSpecificationBuilder(Specification<T> specification, LambdaExpression parent)
        : base(specification)
    {
        _parent = parent;
    }

    public IIncludableSpecificationBuilder<T, TNext> ThenInclude<TNext>(Expression<Func<TElement, TNext>> navigation)
    {
        _specification.AddThenInclude(navigation, _parent);
        return new IncludableSpecificationBuilder<T, TNext>(_specification, navigation);
    }
}
