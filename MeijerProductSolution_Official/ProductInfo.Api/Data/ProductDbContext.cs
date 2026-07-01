using Microsoft.EntityFrameworkCore;
using ProductInfo.Api.Models;

namespace ProductInfo.Api.Data;

public sealed class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(product => product.Id);
            entity.Property(product => product.Id).ValueGeneratedNever();
            entity.Property(product => product.Title).IsRequired().HasMaxLength(200);
            entity.Property(product => product.Summary).IsRequired().HasMaxLength(500);
            entity.Property(product => product.Description).IsRequired().HasMaxLength(2000);
            entity.Property(product => product.Price).IsRequired().HasMaxLength(50);
            entity.Property(product => product.ImageUrl).IsRequired().HasMaxLength(1000);
        });
    }
}
