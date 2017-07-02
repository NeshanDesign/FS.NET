using System;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using FsNet.Data.Contracts.Infrastructure;
using LinqKit;

namespace FsNet.Data.EF
{
    public abstract class PredicateBinder<TEntity> : IPredicateBinder<TEntity>
    {
        private readonly Expression<Func<TEntity, bool>> _query;

        protected PredicateBinder(Expression<Func<TEntity, bool>> query)
        {
            _query = query;
        }

        public virtual Expression<Func<TEntity, bool>> Query()
        {
            return _query;
        }

        public Expression<Func<TEntity, bool>> And(Expression<Func<TEntity, bool>> query)
        {
            Contract.Assert(_query != null);
            return  _query.And(query.Expand());
        }

        public Expression<Func<TEntity, bool>> Or(Expression<Func<TEntity, bool>> query)
        {
            Contract.Assert(_query != null);
            return  _query.Or(query.Expand());
        }

        public Expression<Func<TEntity, bool>> And(IPredicateBinder<TEntity> predicateBinder)
        {
            return And(predicateBinder.Query());
        }

        public Expression<Func<TEntity, bool>> Or(IPredicateBinder<TEntity> predicateBinder)
        {
            return Or(predicateBinder.Query());
        }
    }
}