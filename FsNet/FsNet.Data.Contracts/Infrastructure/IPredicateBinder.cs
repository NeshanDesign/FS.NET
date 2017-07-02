using System;
using System.Linq.Expressions;

namespace FsNet.Data.Contracts.Infrastructure
{
    public interface IPredicateBinder<TEntity>
    {
        Expression<Func<TEntity, bool>> Query();
        Expression<Func<TEntity, bool>> And(Expression<Func<TEntity, bool>> query);
        Expression<Func<TEntity, bool>> Or(Expression<Func<TEntity, bool>> query);
        Expression<Func<TEntity, bool>> And(IPredicateBinder<TEntity> predicateBinder);
        Expression<Func<TEntity, bool>> Or(IPredicateBinder<TEntity> predicateBinder);
    }
}