using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FsNet.Data.Contracts.Infrastructure;
using FsNet.Data.Contracts.Model;

namespace FsNet.Data.Contracts.Repository
{
    public interface IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        TEntity Find(params object[] keyValues);
        Task<TEntity> FindAsync(params object[] keyValues);
        Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues);
        IQueryable<TEntity> SelectQuery(string query, params object[] parameters);
        void Insert(TEntity entity);
        void InsertRange(IEnumerable<TEntity> entities);
        void AddRange(IEnumerable<TEntity> entities);
        void InsertOrUpdate(TEntity entity);
        void Update(TEntity entity);
        void Delete(object id);
        void Delete(TEntity entity);
        Task<bool> DeleteAsync(params object[] keyValues);
        Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues);
        IQueryFluent<TEntity, TKey> Query(IPredicateBinder<TEntity> predicateBinder);
        IQueryFluent<TEntity, TKey> Query(Expression<Func<TEntity, bool>> query);
        IQueryFluent<TEntity, TKey> Query();
        IQueryable<TEntity> Queryable();
    }
}