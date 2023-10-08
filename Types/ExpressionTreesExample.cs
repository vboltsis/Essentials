using System.Linq.Expressions;

namespace FeatureExamples;

public class ExpressionTreesExample
{
    public static void ExpressionTreeAdd()
    {
        var paramA = Expression.Parameter(typeof(int), "a");
        var paramB = Expression.Parameter(typeof(int), "b");
        var body = Expression.Add(paramA, paramB);
        var addExpression = Expression.Lambda<Func<int, int, int>>(body, paramA, paramB);
        var compiled = addExpression.Compile();

        Console.WriteLine(compiled(1, 2));  // Outputs: 3
    }

    public static void LinqExample()
    {
        List<Product> products = new()
        {
            new Product { Name = "Laptop", Price = 1000M },
            new Product { Name = "Mouse", Price = 20M },
            new Product { Name = "Keyboard", Price = 50M }
        };

        // Dynamic filter criteria
        string userInputName = "Laptop";  // Example user input
        decimal? userInputPrice = null;  // No price filter

        // Building the expression tree
        var param = Expression.Parameter(typeof(Product), "p");
        Expression predicate = Expression.Constant(true);  // Default to true for AND operations

        if (!string.IsNullOrEmpty(userInputName))
        {
            var nameProperty = Expression.Property(param, "Name");
            var nameValue = Expression.Constant(userInputName);
            var nameEquals = Expression.Equal(nameProperty, nameValue);
            predicate = Expression.AndAlso(predicate, nameEquals);
        }

        if (userInputPrice.HasValue)
        {
            var priceProperty = Expression.Property(param, "Price");
            var priceValue = Expression.Constant(userInputPrice.Value);
            var priceEquals = Expression.Equal(priceProperty, priceValue);
            predicate = Expression.AndAlso(predicate, priceEquals);
        }

        var lambda = Expression.Lambda<Func<Product, bool>>(predicate, param);
        var filteredProducts = products.AsQueryable().Where(lambda).ToList();

        foreach (var product in filteredProducts)
        {
            Console.WriteLine(product);
        }
    }
}

public record Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}
