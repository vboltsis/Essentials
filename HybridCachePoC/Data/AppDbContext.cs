using HybridCachePoC.Models;
using Microsoft.EntityFrameworkCore;

namespace HybridCachePoC.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()");
            
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255);
            
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20);
            
            entity.Property(e => e.Company)
                .HasMaxLength(100);
            
            entity.Property(e => e.Address)
                .HasMaxLength(255);
            
            entity.Property(e => e.City)
                .HasMaxLength(100);
            
            entity.Property(e => e.State)
                .HasMaxLength(50);
            
            entity.Property(e => e.PostalCode)
                .HasMaxLength(20);
            
            entity.Property(e => e.Country)
                .HasMaxLength(100);
            
            entity.Property(e => e.Notes)
                .HasMaxLength(500);
            
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            
            entity.Property(e => e.AccessCount)
                .HasDefaultValue(0);
            
            entity.Property(e => e.IsActive)
                .HasDefaultValue(true);
            
            // Create unique index on email
            entity.HasIndex(e => e.Email)
                .IsUnique();
            
            // Create index on last accessed for performance
            entity.HasIndex(e => e.LastAccessed);
            
            // Create index on access count for metrics
            entity.HasIndex(e => e.AccessCount);
        });
    }
} 