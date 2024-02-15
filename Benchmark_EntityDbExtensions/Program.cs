using Benchmark_EntityDbExtensions;
using Benchmark_EntityDbExtensions.Context;
using BenchmarkDotNet.Running;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

class Program
{
    public static IHostBuilder CreateHostBuilder(string[] args) => 
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddDbContext<SqlLiteDbContext>((sp, options) =>
                {
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    options.UseSqlite("Data Source=Benchmark_EntityDbExtensions.db");

                });
                services.AddDbContext<SqlServerDbContext>((sp, options) =>
                {
                    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    // options.UseSqlServer("Server=YOUR_SQL;Database=YOUR_DB;User Id=YOUR_USER;Password=YOUR_PASS;TrustServerCertificate=True;");
                    options.UseSqlServer("Server=AlanBarboza\\SQLEXPRESS;Database=Benchmark_EntityDbExtensions;User Id=sa;Password=123;TrustServerCertificate=True;");
                });
            });

    public static void Main(string[] args) =>  BenchmarkRunner.Run<Benchmark>();
}

