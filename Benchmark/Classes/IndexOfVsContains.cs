namespace Benchmark.Classes;

[MemoryDiagnoser]
public class IndexOfVsContains
{
    [Params(10000)]
    public int Number;
    private List<Permission> _permissions;

    private static List<int> _numbers = new List<int> { 1, 7 };

    [GlobalSetup]
    public void GlobalSetup()
    {
        _permissions = GeneratePermissions(Number);
    }

    [Benchmark(Baseline = true)]
    public List<Permission> GetPermissionsIndexOf()
    {
        return _permissions.Where(x => _numbers.IndexOf(x.RoleId) != -1).ToList();
    }

    [Benchmark]
    public List<Permission> GetPermissionsContains()
    {
        return _permissions.Where(x => _numbers.Contains(x.RoleId)).ToList();
    }

    private static List<Permission> GeneratePermissions(int amount)
    {
        var list = new List<Permission>();
        for (int i = 0; i < amount; i++)
        {
            list.Add(new Permission
            {
                Id = 1,
                Name = i.ToString(),
                RoleId = new Random().Next(1, 10)
            });
        }

        return list;
    }
}

public class Permission
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int RoleId { get; set; }
}
