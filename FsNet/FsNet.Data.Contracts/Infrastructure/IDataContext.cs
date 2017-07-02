using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Threading.Tasks;
using FsNet.Data.Contracts.Model;

namespace FsNet.Data.Contracts.Infrastructure
{
    public interface IDataContext: IDisposable
    {
        int SaveChanges();
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        Task<int> SaveChangesAsync();
        DbSet<T> SetEntity<T>() where T : class;
        DbEntityEntry<IEntity<TKey>> SetEntityEntry<TKey>(IEntity<TKey> entity);
        IDbConnection DbConnection { get; }
       // IDbSet<CustomerAccount> CustomerAccounts { get; }
    }
}