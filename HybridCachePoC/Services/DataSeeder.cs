using HybridCachePoC.Data;
using HybridCachePoC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HybridCachePoC.Services;

public class DataSeeder
{
    private readonly AppDbContext _context;
    private readonly ILogger<DataSeeder> _logger;

    public DataSeeder(AppDbContext context, ILogger<DataSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        try
        {
            // Ensure database is created
            await _context.Database.EnsureCreatedAsync();
            
            // Check if data already exists
            if (await _context.Customers.AnyAsync())
            {
                _logger.LogInformation("Database already contains data, skipping seed");
                return;
            }

            var customers = new List<Customer>
            {
                new Customer
                {
                    Id = Guid.NewGuid(),
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    PhoneNumber = "+1-555-0101",
                    Company = "Tech Corp",
                    Address = "123 Main St",
                    City = "New York",
                    State = "NY",
                    PostalCode = "10001",
                    Country = "USA",
                    Notes = "Premium customer",
                    CreatedAt = DateTime.UtcNow.AddDays(-30),
                    IsActive = true
                },
                new Customer
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    PhoneNumber = "+1-555-0102",
                    Company = "Innovation Inc",
                    Address = "456 Oak Ave",
                    City = "San Francisco",
                    State = "CA",
                    PostalCode = "94102",
                    Country = "USA",
                    Notes = "Enterprise client",
                    CreatedAt = DateTime.UtcNow.AddDays(-25),
                    IsActive = true
                },
                new Customer
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Michael",
                    LastName = "Johnson",
                    Email = "michael.johnson@example.com",
                    PhoneNumber = "+1-555-0103",
                    Company = "StartupXYZ",
                    Address = "789 Pine St",
                    City = "Austin",
                    State = "TX",
                    PostalCode = "73301",
                    Country = "USA",
                    Notes = "New startup customer",
                    CreatedAt = DateTime.UtcNow.AddDays(-20),
                    IsActive = true
                },
                new Customer
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Sarah",
                    LastName = "Williams",
                    Email = "sarah.williams@example.com",
                    PhoneNumber = "+1-555-0104",
                    Company = "Global Solutions",
                    Address = "321 Elm Rd",
                    City = "Chicago",
                    State = "IL",
                    PostalCode = "60601",
                    Country = "USA",
                    Notes = "International operations",
                    CreatedAt = DateTime.UtcNow.AddDays(-15),
                    IsActive = true
                },
                new Customer
                {
                    Id = Guid.NewGuid(),
                    FirstName = "David",
                    LastName = "Brown",
                    Email = "david.brown@example.com",
                    PhoneNumber = "+1-555-0105",
                    Company = "Digital Dynamics",
                    Address = "654 Maple Dr",
                    City = "Seattle",
                    State = "WA",
                    PostalCode = "98101",
                    Country = "USA",
                    Notes = "Tech enthusiast",
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    IsActive = true
                }
            };

            await _context.Customers.AddRangeAsync(customers);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully seeded {CustomerCount} customers", customers.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error seeding database");
            throw;
        }
    }
} 