using E_Commerce.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace E_Commerce.Service.Specifications
{
    public class BaseSpecification<TEntity> : ISpecification<TEntity> where TEntity : class
    {
        public BaseSpecification(Expression<Func<TEntity, bool>>? criteria)
        {
            Criteria = criteria;
        }
        public Expression<Func<TEntity, bool>>? Criteria { get; private set; }

        public ICollection<Expression<Func<TEntity, object>>> Includes { get; private set; } = new List<Expression<Func<TEntity, object>>>();

        public Expression<Func<TEntity, object>>? OrderBy { get; protected set; }

        public Expression<Func<TEntity, object>>? OrderByDescending { get; protected set; }

        public int skip { get; private set; }

        public int take { get; private set; }

        public bool isPaginated { get; private set; }

        protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            if (Includes == null)
            {
                Includes = new List<Expression<Func<TEntity, object>>>();
            }
            Includes.Add(includeExpression);
        }

        public void ApplyPaging(int pageSize, int pageIndex)
        {
            isPaginated = true;
            
            take = pageSize;
            skip = pageSize * (pageIndex - 1);
        }
    }
}
