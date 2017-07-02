using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FsNet.Data.Contracts.Model;

namespace FsNet.Data.Contracts.Infrastructure
{
    public interface IQueryFluent<TEntity,TKey> where TEntity : class, IEntity<TKey>
    {
        IQueryFluent<TEntity, TKey> OrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy);
        IQueryFluent<TEntity, TKey> Include(Expression<Func<TEntity, object>> expression);
        IEnumerable<TEntity> Get(int page, int pageSize, out int totalCount);
        IEnumerable<TResult> Select<TResult>(Expression<Func<TEntity, TResult>> selector = null);
        IEnumerable<TEntity> Select();
        Task<IEnumerable<TEntity>> SelectAsync();
        IQueryable<TEntity> SqlQuery(string query, params object[] parameters);
    }
}