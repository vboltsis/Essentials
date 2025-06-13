using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkPlayground;

public class BasicExample
{
    public static async Task Example()
    {
        using var db = new AppDbContext();

        var products = await db.Products
            .AsNoTrackingWithIdentityResolution()
            .Where(p => p.Price > 100)
            .ToListAsync();
        
        // Create a new product instance
        var newProduct = new Product { Name = "Banana", Price = 0.30m };

        // Add the product to the context
        db.Products.Add(newProduct);

        // Inspect the change tracker entries before saving
        Console.WriteLine("Before SaveChanges:");
        foreach (var entry in db.ChangeTracker.Entries())
        {
            Console.WriteLine($"Entity: {entry.Entity.GetType().Name}, State: {entry.State}");
        }

        // Save changes to the database
        await db.SaveChangesAsync();

        // Inspect the change tracker entries after saving
        Console.WriteLine("\nAfter SaveChanges:");
        foreach (var entry in db.ChangeTracker.Entries())
        {
            Console.WriteLine($"Entity: {entry.Entity.GetType().Name}, State: {entry.State}");
        }

        // Modify the product
        newProduct.Price = 0.35m;

        // Inspect the change tracker after modification
        Console.WriteLine("\nAfter Modification:");
        foreach (var entry in db.ChangeTracker.Entries())
        {
            Console.WriteLine($"Entity: {entry.Entity.GetType().Name}, State: {entry.State}");
        }

        // Save the updated product
        await db.SaveChangesAsync();

        // Delete the product
        db.Products.Remove(newProduct);

        // Inspect the change tracker after deletion
        Console.WriteLine("\nAfter Deletion:");
        foreach (var entry in db.ChangeTracker.Entries())
        {
            Console.WriteLine($"Entity: {entry.Entity.GetType().Name}, State: {entry.State}");
        }

        // Save the deletion
        await db.SaveChangesAsync();
    }
}
