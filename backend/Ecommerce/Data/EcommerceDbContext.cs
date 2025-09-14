using Microsoft.EntityFrameworkCore;
using Ecommerce.Entity; 

namespace Ecommerce.Data;

public class EcommerceDbContext : DbContext
{
    public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : base(options)
    {
    }
    public DbSet<Product> Products { get; set; } 

    
}