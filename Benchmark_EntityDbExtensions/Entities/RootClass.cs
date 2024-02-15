namespace Benchmark_EntityDbExtensions.Entities;

public class RootClass
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public virtual IEnumerable<ChildClass> ChildClass { get; set; }
}
