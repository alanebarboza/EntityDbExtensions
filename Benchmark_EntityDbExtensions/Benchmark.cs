using Benchmark_EntityDbExtensions.Context;
using Benchmark_EntityDbExtensions.Entities;
using BenchmarkDotNet.Attributes;
using Microsoft.EntityFrameworkCore;

using EntityDbExtensions;

namespace Benchmark_EntityDbExtensions
{
    [MemoryDiagnoser]
    public class Benchmark
    {
        private SqlLiteDbContext _sqlLiteDbContext;
        private SqlServerDbContext _sqlServerDbContext;

        [GlobalSetup]
        public void Setup()
        {
            _sqlLiteDbContext = new SqlLiteDbContext(new DbContextOptions<SqlLiteDbContext>());
            _sqlLiteDbContext.Database.Migrate();

            _sqlServerDbContext = new SqlServerDbContext(new DbContextOptions<SqlServerDbContext>());
            _sqlServerDbContext.Database.Migrate();
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            _sqlLiteDbContext.Dispose();
            _sqlServerDbContext.Dispose();
        }

        [Benchmark]
        public async Task Default_EntityUpdate_Benchmark_SQLLite()
        {
            for (int i = 0; i < 10; i++)
            {
                await PrepareDatabase(_sqlLiteDbContext);

                var RootClassDb = GetRootClassDb();
                var RootClassUpdate = GetRootClassUpdated();

                foreach (var ChildClassDb in RootClassDb.ChildClass)
                {
                    if (!RootClassUpdate.ChildClass.Any(c => c.Id == ChildClassDb.Id))
                    {
                        _sqlLiteDbContext.Entry(ChildClassDb).State = EntityState.Deleted;
                        continue;
                    }

                    foreach (var GrandChildClassUpdate in ChildClassDb.GrandChildClass)
                    {
                        if (!RootClassUpdate.ChildClass.First(x => x.Id == ChildClassDb.Id).GrandChildClass.Any(c => c.Id == GrandChildClassUpdate.Id))
                            _sqlLiteDbContext.Entry(GrandChildClassUpdate).State = EntityState.Deleted;
                    }
                }

                _sqlLiteDbContext.RootClass.Update(RootClassUpdate);

                await _sqlLiteDbContext.SaveChangesAsync();

                await ClearDatabase(_sqlLiteDbContext, RootClassUpdate);
            }
        }
        [Benchmark]
        public async Task EntityDbExtensions_UpdateAndHandleDeletedChildren_Benchmark_SQLLite()
        {
            for (int i = 0; i < 10; i++)
            {
                await PrepareDatabase(_sqlLiteDbContext);
                var RootClassDb = GetRootClassDb();
                var RootClassUpdate = GetRootClassUpdated();

                await _sqlLiteDbContext.UpdateAndHandleDeletedChildren(RootClassUpdate, RootClassDb); // EntityDbExtensions
              
                await _sqlLiteDbContext.SaveChangesAsync();
                await ClearDatabase(_sqlLiteDbContext, RootClassUpdate);
            }
        }

        [Benchmark]
        public async Task Default_EntityUpdate_Benchmark_SQLServer()
        {
            for (int i = 0; i < 10; i++)
            {
                await PrepareDatabase(_sqlServerDbContext);
                var RootClassDb = GetRootClassDb();
                var RootClassUpdate = GetRootClassUpdated();

                foreach (var ChildClassDb in RootClassDb.ChildClass)
                {
                    if (!RootClassUpdate.ChildClass.Any(c => c.Id == ChildClassDb.Id))
                    {
                        _sqlServerDbContext.Entry(ChildClassDb).State = EntityState.Deleted;
                        continue;
                    }

                    foreach (var GrandChildClassUpdate in ChildClassDb.GrandChildClass)
                    {
                        if (!RootClassUpdate.ChildClass.First(x => x.Id == ChildClassDb.Id).GrandChildClass.Any(c => c.Id == GrandChildClassUpdate.Id))
                            _sqlServerDbContext.Entry(GrandChildClassUpdate).State = EntityState.Deleted;
                    }
                }

                _sqlServerDbContext.RootClass.Update(RootClassUpdate);

                await _sqlServerDbContext.SaveChangesAsync();
                await ClearDatabase(_sqlServerDbContext, RootClassUpdate);
            }
        }

        [Benchmark]
        public async Task EntityDbExtensions_UpdateAndHandleDeletedChildren_Benchmark_SQLServer()
        {
            for (int i = 0; i < 10; i++)
            {
                await PrepareDatabase(_sqlServerDbContext);
                var RootClassDb = GetRootClassDb();
                var RootClassUpdate = GetRootClassUpdated();

                await _sqlServerDbContext.UpdateAndHandleDeletedChildren(RootClassUpdate, RootClassDb); // EntityDbExtensions

                _sqlServerDbContext.Update(RootClassUpdate);

                await _sqlServerDbContext.SaveChangesAsync();
                await ClearDatabase(_sqlServerDbContext, RootClassUpdate);
            }
        }

        public RootClass GetRootClassDb()
        {
            return new RootClass()
            {
                Id = Guid.Parse("E8A8381E-402D-43DE-97E8-E720A79B689B"),
                Name = "RootClass 1",
                Age = 50,
                ChildClass = new List<ChildClass>()
            {   new()
                {
                    Id = Guid.Parse("F242CA0A-4785-4C34-B395-0B7A45929EAD"),
                    RootClassId = Guid.Parse("E8A8381E-402D-43DE-97E8-E720A79B689B"),
                    Name = "ChildClass 1",
                    Age = 30,
                    GrandChildClass = new List<GrandChildClass>()
                    {   new()
                        {
                            Id = Guid.Parse("B42C82F9-D419-44BE-8A9A-08E063341FD6"),
                            ChildClassId= Guid.Parse("F242CA0A-4785-4C34-B395-0B7A45929EAD"),
                            Name = "GrandChildClass 1",
                            Age = 10,
                        },
                        new()
                        {
                            Id = Guid.Parse("8802AB83-7AA5-45BA-8D48-8A4A4CF9F42E"),
                            ChildClassId= Guid.Parse("F242CA0A-4785-4C34-B395-0B7A45929EAD"),
                            Name = "GrandChildClass 2",
                            Age = 11,
                        },
                        new()
                        {
                            Id = Guid.Parse("5DCEB46F-EB36-4547-AE3D-A7847421AE7C"),
                            ChildClassId= Guid.Parse("F242CA0A-4785-4C34-B395-0B7A45929EAD"),
                            Name = "GrandChildClass 3",
                            Age = 12,
                        },
                        new()
                        {
                            Id = Guid.Parse("8F4F7F0C-CF81-4DEF-9964-B6BC39DFCED4"),
                            ChildClassId= Guid.Parse("F242CA0A-4785-4C34-B395-0B7A45929EAD"),
                            Name = "GrandChildClass 4",
                            Age = 13,
                        },
                    }
                },
                new()
                {
                    Id = Guid.Parse("ECCC1144-49A3-43A3-AC1D-AA68D7969162"),
                    RootClassId = Guid.Parse("E8A8381E-402D-43DE-97E8-E720A79B689B"),
                    Name = "ChildClass 2",
                    Age = 60,
                    GrandChildClass = new List<GrandChildClass>()
                    {   new()
                        {
                            Id = Guid.Parse("E6D391FC-3944-47CD-8BDB-0EBFCAADB93B"),
                            ChildClassId= Guid.Parse("ECCC1144-49A3-43A3-AC1D-AA68D7969162"),
                            Name = "GrandChildClass 1",
                            Age = 40,
                        },
                        new()
                        {
                            Id = Guid.Parse("DD40EF8F-151B-4603-AC59-7EB00C643BC1"),
                            ChildClassId= Guid.Parse("ECCC1144-49A3-43A3-AC1D-AA68D7969162"),
                            Name = "GrandChildClass 2",
                            Age = 30,
                        },
                        new()
                        {
                            Id = Guid.Parse("63D0E5CD-0476-46A9-BB6F-B51947D1A72F"),
                            ChildClassId= Guid.Parse("ECCC1144-49A3-43A3-AC1D-AA68D7969162"),
                            Name = "GrandChildClass 3",
                            Age = 20,
                        },
                        new()
                        {
                            Id = Guid.Parse("8ED8712A-B97B-4ABC-826A-D0883C3CA665"),
                            ChildClassId= Guid.Parse("ECCC1144-49A3-43A3-AC1D-AA68D7969162"),
                            Name = "GrandChildClass 4",
                            Age = 10,
                        },
                    }
                },
                new()
                {
                    Id = Guid.Parse("A3A8ADDA-51B6-4287-8F58-61F76354BFEC"),
                    RootClassId = Guid.Parse("E8A8381E-402D-43DE-97E8-E720A79B689B"),
                    Name = "ChildClass 3",
                    Age = 80,
                    GrandChildClass = new List<GrandChildClass>()
                    {   new()
                        {
                            Id = Guid.Parse("2302BF0D-DB6C-4CCB-806E-A7FD1B91C42C"),
                            ChildClassId= Guid.Parse("A3A8ADDA-51B6-4287-8F58-61F76354BFEC"),
                            Name = "GrandChildClass 1",
                            Age = 60,
                        },
                        new()
                        {
                            Id = Guid.Parse("BBF1253C-7DD1-4ADF-ACDC-D55889B85BA4"),
                            ChildClassId= Guid.Parse("A3A8ADDA-51B6-4287-8F58-61F76354BFEC"),
                            Name = "GrandChildClass 2",
                            Age = 50,
                        },
                        new()
                        {
                            Id = Guid.Parse("C849EC00-B10F-4588-ADB1-CD6232250697"),
                            ChildClassId= Guid.Parse("A3A8ADDA-51B6-4287-8F58-61F76354BFEC"),
                            Name = "GrandChildClass 3",
                            Age = 60,
                        },
                        new()
                        {
                            Id = Guid.Parse("F393D4AF-B290-4DFD-8695-55EA0365887B"),
                            ChildClassId= Guid.Parse("A3A8ADDA-51B6-4287-8F58-61F76354BFEC"),
                            Name = "GrandChildClass 4",
                            Age = 50,
                        },
                    }
                }
            }
            };
        }
        public RootClass GetRootClassUpdated()
        {
            return new RootClass()
            {
                Id = Guid.Parse("E8A8381E-402D-43DE-97E8-E720A79B689B"),
                Name = "RootClass 1 - Updated - 1 ChildClass Removed",
                Age = 50,
                ChildClass = new List<ChildClass>()
            {   new()
                {
                    Id = Guid.Parse("F242CA0A-4785-4C34-B395-0B7A45929EAD"),
                    RootClassId = Guid.Parse("E8A8381E-402D-43DE-97E8-E720A79B689B"),
                    Name = "ChildClass 1 - Updated - 3 GrandChildClass Removed",
                    Age = 30,
                    GrandChildClass = new List<GrandChildClass>()
                    {   new()
                        {
                            Id = Guid.Parse("B42C82F9-D419-44BE-8A9A-08E063341FD6"),
                            ChildClassId= Guid.Parse("F242CA0A-4785-4C34-B395-0B7A45929EAD"),
                            Name = "GrandChildClass 1",
                            Age = 10,
                        }
                    }
                },
                new()
                {
                    Id = Guid.Parse("ECCC1144-49A3-43A3-AC1D-AA68D7969162"),
                    RootClassId = Guid.Parse("E8A8381E-402D-43DE-97E8-E720A79B689B"),
                    Name = "ChildClass 2 - Updated - 3 GrandChildClass Removed",
                    Age = 60,
                    GrandChildClass = new List<GrandChildClass>()
                    {   new()
                        {
                            Id = Guid.Parse("E6D391FC-3944-47CD-8BDB-0EBFCAADB93B"),
                            ChildClassId= Guid.Parse("ECCC1144-49A3-43A3-AC1D-AA68D7969162"),
                            Name = "GrandChildClass 1",
                            Age = 40,
                        }
                    }
                }
            }
            };
        }

        public async Task PrepareDatabase(IDbContext dbContext)
        {
            var rootClass = GetRootClassDb();
            await dbContext.RootClass.AddAsync(rootClass);
            await dbContext.SaveChangesAsync(default);
            dbContext.Entry(rootClass).State = EntityState.Detached;
        }
        public async Task ClearDatabase(IDbContext dbContext, RootClass RootClassUpdate)
        {
            dbContext.RootClass.Remove(RootClassUpdate);
            await dbContext.SaveChangesAsync(default);
            dbContext.Entry(RootClassUpdate).State = EntityState.Detached;
        }
    }
}
