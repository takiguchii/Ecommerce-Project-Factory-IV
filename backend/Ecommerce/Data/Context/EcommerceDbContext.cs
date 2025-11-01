using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Entity;

namespace Ecommerce.Data.Context
{
    public class EcommerceDbContext : IdentityDbContext<IdentityUser>
    {
        public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user_profile"); 
                entity.HasKey(u => u.Id);
                
                entity.HasOne(u => u.AppUser)
                    .WithOne() 
                    .HasForeignKey<User>(u => u.AppUserId)
                    .IsRequired();
                
                entity.HasIndex(u => u.AppUserId).IsUnique();
            });
            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("brand");
                entity.HasKey(b => b.id);
            });

            modelBuilder.Entity<Provider>(entity =>
            {
                entity.ToTable("provider");
                entity.HasKey(p => p.id);
            });
            
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("category");
                entity.HasKey(c => c.id);
            });

            modelBuilder.Entity<SubCategory>(entity =>
            {
                entity.ToTable("sub_category");
                entity.HasKey(sc => sc.id);
                
                entity.HasOne(sc => sc.ParentCategory) 
                      .WithMany() 
                      .HasForeignKey(sc => sc.category_id) 
                      .HasPrincipalKey(c => c.id); 
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("product"); 
                entity.HasKey(p => p.id); 
                entity.HasOne(p => p.Category)         
                      .WithMany(c => c.Products)    
                      .HasForeignKey(p => p.category_id)
                      .HasPrincipalKey(c => c.id);    

                entity.HasOne(p => p.SubCategory)       
                      .WithMany(sc => sc.Products)    
                      .HasForeignKey(p => p.sub_category_id)
                      .HasPrincipalKey(sc => sc.id);     

                entity.HasOne(p => p.Brand)        
                      .WithMany(b => b.Products)     
                      .HasForeignKey(p => p.brand_id)    
                      .HasPrincipalKey(b => b.id);     

                entity.HasOne(p => p.Provider)          
                      .WithMany(pr => pr.Products)    
                      .HasForeignKey(p => p.provider_id) 
                      .HasPrincipalKey(p => p.id);     
            });
            

        } 
    } 
} 

