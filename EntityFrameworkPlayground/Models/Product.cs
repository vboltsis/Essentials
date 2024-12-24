namespace EntityFrameworkPlayground;

public class Product
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public decimal Price { get; set; }

    public Type Type { get; set; }
}

public class Type
{
    public string Name { get; set; }
    public int Id { get; set; }
}