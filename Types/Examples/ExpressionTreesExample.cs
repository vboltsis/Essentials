using System.Linq.Expressions;

namespace FeatureExamples;

public class ExpressionTreesExample
{
    public static void ExpressionTreeAdd(int a, int b)
    {
        var paramA = Expression.Parameter(typeof(int), "a");
        var paramB = Expression.Parameter(typeof(int), "b");
        var body = Expression.Add(paramA, paramB);
        var addExpression = Expression.Lambda<Func<int, int, int>>(body, paramA, paramB);
        Func<int, int, int> compiled = addExpression.Compile();

        Console.WriteLine(compiled(a, b));  // Outputs the sum of a and b
    }

    public static void LinqExample()
    {
        List<Product> products = new()
        {
            new Product { Name = "Laptop", Price = 1000M },
            new Product { Name = "Mouse", Price = 20M },
            new Product { Name = "Keyboard", Price = 50M },
            new Product { Name = "Laptop", Price = 300M }
        };

        // Dynamic filter criteria
        string nameFilter = "Laptop";  // Example user input
        decimal? userInputPrice = null;  // No price filter

        // Building the expression tree
        var productParamExpression = Expression.Parameter(typeof(Product), "p");
        Expression predicate = Expression.Constant(true);  // Default to true for AND operations

        if (!string.IsNullOrEmpty(nameFilter))
        {
            var nameProperty = Expression.Property(productParamExpression, "Name");
            var nameValue = Expression.Constant(nameFilter);
            var nameEquals = Expression.Equal(nameProperty, nameValue);
            predicate = Expression.AndAlso(predicate, nameEquals);
        }

        if (userInputPrice.HasValue)
        {
            var priceProperty = Expression.Property(productParamExpression, "Price");
            var priceValue = Expression.Constant(userInputPrice.Value);
            var priceEquals = Expression.GreaterThanOrEqual(priceProperty, priceValue);
            predicate = Expression.AndAlso(predicate, priceEquals);
        }

        var lambda = Expression.Lambda<Func<Product, bool>>(predicate, productParamExpression);
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
