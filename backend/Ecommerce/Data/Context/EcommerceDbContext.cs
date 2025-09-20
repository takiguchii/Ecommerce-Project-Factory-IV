using Microsoft.EntityFrameworkCore;
using Ecommerce.Entity; 

namespace Ecommerce.Data;

public class EcommerceDbContext : DbContext
{
    public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : base(options)
    {
    }
    
    public DbSet<Product> Products { get; set; } 
    public DbSet<Category> Categories { get; set; }
    public DbSet<SubCategory> SubCategories { get; set; }
    public DbSet<Provider> Providers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Relação do produto com a categoria ( Um para muitos ) 
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c=> c.Products)
            .HasForeignKey(p => p.CategoryId);
        
        base.OnModelCreating(modelBuilder);
    }



    
}