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





    
}