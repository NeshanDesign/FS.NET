using System;
using System.Threading;
using System.Threading.Tasks;

namespace FsNet.Data.Contracts.Infrastructure
{
    public interface IUnitOfWork : IDisposable
    {
        IDataContext DbContext { get; }
        int Commit();
        Task<int> CommitAsync();
        Task<int> CommitAsync(CancellationToken cancellationToken);

        void Rollback();
        Task RollbackAsync();
    }
}