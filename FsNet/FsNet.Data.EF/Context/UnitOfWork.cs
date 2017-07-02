#region

using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using FsNet.Common.Utils;
using FsNet.Data.Contracts.Infrastructure;

#endregion

namespace FsNet.Data.EF.Context
{
    public class UnitOfWork : IUnitOfWork
    {
        public IDataContext DbContext { get; internal set; }
        private readonly bool _autoCommit;
        private bool _disposed;
        private IsolationLevel _isolationLevel;
        private IDbTransaction _transaction;
        private bool _isCompleted = false;


        public UnitOfWork() : this(new DataContext(), IsolationLevel.ReadCommitted, true)
        {

        }

        public UnitOfWork(bool autoCommit) : this(new DataContext(), IsolationLevel.ReadCommitted, autoCommit)
        {

        }

        public UnitOfWork(IDataContext dbContext) : this(dbContext, IsolationLevel.ReadCommitted, true)
        {

        }

        public UnitOfWork(IDataContext dbContext, bool autoCommit) : this(dbContext, IsolationLevel.ReadCommitted, autoCommit)
        {

        }

        public UnitOfWork(IDataContext dbContext, IsolationLevel isolationLevel, bool autoCommit)
        {
            DbContext = dbContext;
            _autoCommit = autoCommit;
            _isolationLevel = isolationLevel;
            OpenConnection();
            _transaction = DbContext.DbConnection.BeginTransaction(isolationLevel);
        }

        public virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                try
                {
                    try
                    {
                        if (_transaction != null && !_isCompleted)
                        {
                            if (_autoCommit)
                                _transaction.Commit();
                            else
                                _transaction.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                    }

                    try
                    {
                        if (DbContext != null && DbContext.DbConnection.State == ConnectionState.Open)
                        {
                            DbContext.DbConnection.Close();
                        }
                    }
                    catch (ObjectDisposedException ode)
                    {
                        Logger.Error(ode);
                        // do nothing
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(ex);
                        // do nothing
                    }
                }
                finally
                {
                    ReleaseCurrentTransaction();
                }

                if (DbContext != null)
                {
                    DbContext.Dispose();
                    DbContext = null;
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public int Commit()
        {
            var result = DbContext.SaveChanges();
            _transaction.Commit();
            _isCompleted = true;
            return result;
        }

        public async Task<int> CommitAsync()
        {
            var result = await DbContext.SaveChangesAsync();
            _transaction.Commit();
            _isCompleted = true;
            return result;
        }

        public async Task<int> CommitAsync(CancellationToken cancellationToken)
        {
            var result = await DbContext.SaveChangesAsync(cancellationToken);
            if (!cancellationToken.IsCancellationRequested)
                _transaction.Commit();
            else
                _transaction.Rollback();
            _isCompleted = true;
            return result;
        }

        public void Rollback()
        {
            _transaction.Rollback();
            _isCompleted = true;
        }

        public  Task RollbackAsync()
        {
            return new Task(() =>
            {
                _transaction.Rollback();
                _isCompleted = true;
            });
        }

        private void OpenConnection()
        {
            if (DbContext.DbConnection.State != ConnectionState.Open)
            {
                DbContext.DbConnection.Open();
            }
        }

        private void ReleaseCurrentTransaction()
        {
            if (_transaction != null)
            {
                try
                {
                    _transaction.Dispose();
                    _transaction = null;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    //do nothing
                }
            }
        }
    }
}