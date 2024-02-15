using Benchmark_EntityDbExtensions.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Benchmark_EntityDbExtensions.Context;

public class SqlServerDbContext : DbContext, IDbContext
{
    public DbSet<RootClass> RootClass { get; set; }
    public DbSet<ChildClass> ChildClass { get; set; }
    public DbSet<GrandChildClass> GrandChildClass { get; set; }

    public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options) : base(options){}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        // optionsBuilder.UseSqlServer("Server=YOUR_SQL;Database=YOUR_DB;User Id=YOUR_USER;Password=YOUR_PASS;TrustServerCertificate=True;");
        optionsBuilder.UseSqlServer("Server=AlanBarboza\\SQLEXPRESS;Database=Benchmark_EntityDbExtensions;User Id=sa;Password=123;TrustServerCertificate=True;");

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}
