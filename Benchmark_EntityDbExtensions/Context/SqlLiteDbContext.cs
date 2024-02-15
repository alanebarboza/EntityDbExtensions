using Benchmark_EntityDbExtensions.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Benchmark_EntityDbExtensions.Context;

public class SqlLiteDbContext : DbContext, IDbContext
{
    public DbSet<RootClass> RootClass { get; set; }
    public DbSet<ChildClass> ChildClass { get; set; }
    public DbSet<GrandChildClass> GrandChildClass { get; set; }

    public SqlLiteDbContext(DbContextOptions<SqlLiteDbContext> options) : base(options){}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        optionsBuilder.UseSqlite("Data Source=Benchmark_EntityDbExtensions.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
