using System;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FsNet.Data.Contracts.Infrastructure;
using FsNet.Data.Contracts.Model;
using FsNet.Data.Domain.User;
using FsNet.Data.EF.Migrations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FsNet.Data.EF.Context
{
    public class DataContext : IdentityDbContext<ApplicationUser>, IDataContext, IObjectContextAdapter
    {
        bool _disposed;
        public Guid UniqueId { get; internal set; }

        private const string CreationDate = "CreationDate";
        private const string ModifiedDate = "ModifiedDate";

        public IDbConnection DbConnection =>  this.Database.Connection;

        public DataContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            UniqueId = Guid.NewGuid();
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public DataContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            UniqueId = Guid.NewGuid();
            Configuration.LazyLoadingEnabled = false;
            Configuration.ProxyCreationEnabled = false;
        }

        public static DataContext Create()
        {
            var ctx = new DataContext();
            return ctx;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>().ToTable("User");
            modelBuilder.Entity<IdentityUserRole>().ToTable("UserRole");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("UserLogin");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("UserClaim");
            modelBuilder.Entity<IdentityRole>().ToTable("Role");
        }

        public override int SaveChanges()
        {
            UpdateDates();
            var changes = base.SaveChanges();
            return changes;
        }
        
        public override async Task<int> SaveChangesAsync()
        {
            return await this.SaveChangesAsync(CancellationToken.None);
        }

        public DbSet<T> SetEntity<T>() where T : class
        {
            return this.Set<T>();
        }

        public DbEntityEntry<IEntity<T>> SetEntityEntry<T>(IEntity<T> entity)
        {
            return this.Entry<IEntity<T>>(entity);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            UpdateDates();
            try
            {
                var changesAsync = await base.SaveChangesAsync(cancellationToken);
                return changesAsync;
            }
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();

                foreach (var failure in ex.EntityValidationErrors)
                {
                    sb.Append($" {failure.Entry.Entity.GetType()} failed validation {Environment.NewLine}");
                    foreach (var error in failure.ValidationErrors)
                    {
                        sb.Append($" - {error.PropertyName} : {error.ErrorMessage} ");
                        sb.AppendLine();
                    }
                }

                throw new DbEntityValidationException($"Entity Validation Failed : {Environment.NewLine} {sb}", ex.EntityValidationErrors, ex); 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void UpdateDates()
        {
            var entries = ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added) UpdateDates(entry, CreationDate);
                if (entry.State == EntityState.Modified) UpdateDates(entry, ModifiedDate);
            }
        }

        private static void UpdateDates(DbEntityEntry entry, string propName)
        {
            if (entry.Entity.GetType().GetProperty(propName) != null)
                    entry.Property(propName).CurrentValue = DateTime.Now;
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (DbConnection != null && DbConnection.State == ConnectionState.Open)
                        DbConnection.Close();
                }

                _disposed = true;
            }
            base.Dispose(disposing);
        }
    }
}