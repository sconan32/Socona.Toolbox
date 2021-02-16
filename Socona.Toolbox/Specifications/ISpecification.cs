using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Socona.ToolBox.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        List<string> IncludeStrings { get; }

        ISpecification<T> And(ISpecification<T> specification);
        ISpecification<T> And(Expression<Func<T, bool>> predicate);

        ISpecification<T> Or(ISpecification<T> specification);
        ISpecification<T> Or(Expression<Func<T, bool>> predicate);

        Func<IQueryable<T>, IOrderedQueryable<T>> Sort { get; }
        Func<IQueryable<T>, IQueryable<T>> PostProcess { get; }

        ISpecification<T> Take(int amount);
        ISpecification<T> Skip(int amount);

        ISpecification<T> OrderBy<TKey>(Expression<Func<T, TKey>> property);
        ISpecification<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> property);
        T SatisfyingItemFrom(IQueryable<T> query);
        IQueryable<T> SatisfyingItemsFrom(IQueryable<T> query);

        bool IsSatisfied(T item);
      
    }
}
