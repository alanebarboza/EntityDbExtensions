namespace Benchmark_EntityDbExtensions.Entities;

public class ChildClass
{
    public Guid Id { get; set; }
    public Guid RootClassId { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public virtual RootClass RootClass{ get; set; }
    public virtual IEnumerable<GrandChildClass> GrandChildClass { get; set; }
}
