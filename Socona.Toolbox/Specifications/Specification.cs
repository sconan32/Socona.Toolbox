using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Socona.ToolBox.Specifications
{
    public class Specification<T> : ISpecification<T>
    {
        public Specification(Expression<Func<T, bool>> criteria = null)
        {
            Criteria = criteria;
        }
        public Expression<Func<T, bool>> Criteria { get; protected set; }
        public List<Expression<Func<T, object>>> Includes { get; protected set; } = new List<Expression<Func<T, object>>>();
        public List<string> IncludeStrings { get; protected set; } = new List<string>();
        public Func<IQueryable<T>, IOrderedQueryable<T>> Sort { get; protected set; }
        public Func<IQueryable<T>, IQueryable<T>> PostProcess { get; protected set; }

        protected virtual void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }
        protected virtual void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }

        public ISpecification<T> OrderBy<TKey>(Expression<Func<T, TKey>> property)
        {
            var newSpecification = new Specification<T>(Criteria)
            {
                PostProcess = PostProcess,
                Includes = Includes,
                IncludeStrings = IncludeStrings,
            };
            if (Sort != null)
            {
                newSpecification.Sort = items => Sort(items).ThenBy(property);
            }
            else
            {
                newSpecification.Sort = items => items.OrderBy(property);
            }
            return newSpecification;
        }
        public ISpecification<T> OrderByDescending<TKey>(Expression<Func<T, TKey>> property)
        {
            var newSpecification = new Specification<T>(Criteria)
            {
                PostProcess = PostProcess,
                Includes = Includes,
                IncludeStrings = IncludeStrings,
            };
            if (Sort != null)
            {
                newSpecification.Sort = items => Sort(items).ThenByDescending(property);
            }
            else
            {
                newSpecification.Sort = items => items.OrderByDescending(property);
            }
            return newSpecification;
        }

        public ISpecification<T> Take(int amount)
        {
            var newSpecification = new Specification<T>(Criteria)
            {
                Sort = Sort,
                Includes = Includes,
                IncludeStrings = IncludeStrings,

            };
            if (PostProcess != null)
            {
                newSpecification.PostProcess = items => PostProcess(items).Take(amount);
            }
            else
            {
                newSpecification.PostProcess = items => items.Take(amount);
            }
            return newSpecification;
        }

        public ISpecification<T> Skip(int amount)
        {
            var newSpecification = new Specification<T>(Criteria)
            {
                Sort = Sort,
                Includes = Includes,
                IncludeStrings = IncludeStrings,
            };
            if (PostProcess != null)
            {
                newSpecification.PostProcess = items => PostProcess(items).Skip(amount);
            }
            else
            {
                newSpecification.PostProcess = items => items.Skip(amount);
            }
            return newSpecification;
        }

        public ISpecification<T> And(ISpecification<T> specification)
        {
            return new AndSpecification<T>(this, specification);
        }

        public ISpecification<T> And(Expression<Func<T, bool>> predicate)
        {
            return this.And(new Specification<T>(predicate));
        }

        public ISpecification<T> Or(ISpecification<T> specification)
        {
            return new OrSpecification<T>(this, specification);
        }

        public ISpecification<T> Or(Expression<Func<T, bool>> predicate)
        {
            return this.Or(new Specification<T>(predicate));
        }

        private IQueryable<T> Prepare(IQueryable<T> query)
        {
            var filtered = query.Where(Criteria);
            var sorted = Sort(filtered);
            var postProcessed = PostProcess(sorted);
            return postProcessed;
        }

        public T SatisfyingItemFrom(IQueryable<T> query)
        {
            return Prepare(query).SingleOrDefault();
        }

        public IQueryable<T> SatisfyingItemsFrom(IQueryable<T> query)
        {
            return Prepare(query);
        }
    }
}
