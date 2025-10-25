// 1. USINGS ADICIONADOS
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

// Usings existentes
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);








            // Relação Product <-> Category
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);








            //------------------------------------------------------------------------------------------------------------------


            modelBuilder.Entity<Provider>(entity =>
            {
                entity.ToTable("provider");
                entity.Property(p => p.id);
                entity.Property(p => p.name).IsRequired().HasMaxLength(32);
                entity.Property(p => p.cnpj).IsRequired().HasMaxLength(16);
                entity.Property(p => p.email).IsRequired().HasMaxLength(64);
                entity.Property(p => p.phone_number).IsRequired().HasMaxLength(16);
                entity.Property(p => p.address).IsRequired().HasMaxLength(128);
            });

            // Relação Product <-> Provider
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Provider)
                .WithMany(pr => pr.Products)
                .HasForeignKey(p => p.ProviderId);

            //------------------------------------------------------------------------------------------------------------------


            modelBuilder.Entity<SubCategory>(entity =>
            {
                entity.ToTable("sub_category");
                entity.Property(sc => sc.id);
                entity.Property(sc => sc.name).IsRequired().HasMaxLength(32);
                entity.Property(sc => sc.category_id).IsRequired();

                // Relação Product <-> SubCategory
                modelBuilder.Entity<Product>()
                    .HasOne(p => p.SubCategory)
                    .WithMany(sc => sc.Products)
                    .HasForeignKey(p => p.SubCategoryId);

                // Relação SubCategory <-> Category
                modelBuilder.Entity<SubCategory>()
                    .HasOne(sc => sc.ParentCategory)
                    .WithMany()
                    .HasForeignKey(sc => sc.category_id);

                //------------------------------------------------------------------------------------------------------------------
                
                modelBuilder.Entity<Brand>(entity =>
                {
                    entity.ToTable("brand");
                    entity.Property(b => b.id);
                    entity.Property(b => b.name).IsRequired().HasMaxLength(32);
                    entity.Property(b => b.brand_image_url).IsRequired().HasMaxLength(512);
                });
                //Relação de brand <-> Products
                modelBuilder.Entity<Product>()
                    .HasOne(p => p.Brand)
                    .WithMany(b => b.Products)
                    .HasForeignKey(p => p.BrandId);
                //------------------------------------------------------------------------------------------------------------------


            });
        }
    }
}
