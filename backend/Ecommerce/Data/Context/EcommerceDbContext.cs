using Microsoft.EntityFrameworkCore;
using Ecommerce.Entity; 

namespace Ecommerce.Data.Context;

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
        
        // Relação do produto com a SubCategoria ( Um para muitos )
        modelBuilder.Entity<Product>()
            .HasOne(p => p.SubCategory)
            .WithMany(sc => sc.Products)
            .HasForeignKey(p => p.SubCategoryId);
        
        // Relação do produto com o fornecedor ( um para muitos )
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Provider)
            .WithMany(pr => pr.Products)
            .HasForeignKey(p => p.ProviderId);
        
        // Relação de Subcategoria para Categoria ( um para muitos )
        modelBuilder.Entity<SubCategory>()
            .HasOne(sc => sc.ParentCategory)
            .WithMany()
            .HasForeignKey(sc => sc.ParentCategoryId);
        
        
        base.OnModelCreating(modelBuilder);
    }



    
}