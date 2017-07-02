using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FsNet.Data.Contracts.Infrastructure;
using FsNet.Data.Contracts.Model;

namespace FsNet.Data.EF
{ 
    public sealed class QueryFluent<TEntity, TKey> : IQueryFluent<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        #region Private Fields
        private readonly Expression<Func<TEntity, bool>> _expression;
        private readonly List<Expression<Func<TEntity, object>>> _includes;
        private readonly Repository<TEntity, TKey> _repository;
        private Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> _orderBy;
        #endregion Private Fields

        #region Constructors
        public QueryFluent(Repository<TEntity, TKey> repository)
        {
            _repository = repository;
            _includes = new List<Expression<Func<TEntity, object>>>();
        }

        public QueryFluent(Repository<TEntity, TKey> repository, IPredicateBinder<TEntity> predicateBinder) :
            this(repository)
        {
            _expression = predicateBinder.Query();
        }

        public QueryFluent(Repository<TEntity, TKey> repository, Expression<Func<TEntity, bool>> expression) :
            this(repository)
        {
            _expression = expression;
        }
        #endregion Constructors

        public IQueryFluent<TEntity, TKey> OrderBy(Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy)
        {
            _orderBy = orderBy;
            return this;
        }

        public IQueryFluent<TEntity, TKey> Include(Expression<Func<TEntity, object>> expression)
        {
            _includes.Add(expression);
            return this;
        }

        public IEnumerable<TEntity> Get(int page, int pageSize, out int totalCount)
        {
            totalCount = _repository.Select(_expression).Count();
            return _repository.Select(_expression, _orderBy, _includes, page, pageSize);
        }

        public IEnumerable<TEntity> Select()
        {
            return _repository.Select(_expression, _orderBy, _includes);
        }

        public IEnumerable<TResult> Select<TResult>(Expression<Func<TEntity, TResult>> selector)
        {
            return _repository.Select(_expression, _orderBy, _includes).Select(selector);
        }

        public async Task<IEnumerable<TEntity>> SelectAsync()
        {
            return await _repository.SelectAsync(_expression, _orderBy, _includes);
        }

        public IQueryable<TEntity> SqlQuery(string query, params object[] parameters)
        {
            return _repository.SelectQuery(query, parameters).AsQueryable();
        }
    }
}