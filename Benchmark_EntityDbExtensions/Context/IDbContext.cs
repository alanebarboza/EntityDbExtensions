using Benchmark_EntityDbExtensions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Benchmark_EntityDbExtensions.Context
{
    public interface IDbContext
    {
        public DbSet<RootClass> RootClass { get; }
        public DbSet<ChildClass> ChildClass { get; }
        public DbSet<GrandChildClass> GrandChildClass { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    }
}
