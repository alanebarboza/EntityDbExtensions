namespace Benchmark_EntityDbExtensions.Entities;

public class GrandChildClass
{
    public Guid Id { get; set; }
    public Guid ChildClassId { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public virtual ChildClass ChildClass { get; set; }
}
