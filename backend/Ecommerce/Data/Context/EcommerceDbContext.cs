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
            base.OnModelCreating(modelBuilder); // Mantém Identity

            // --- RELAÇÕES ORIGINAIS (MANTIDAS) ---
            // Relação Product <-> Category
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c=> c.Products)
                .HasForeignKey(p => p.CategoryId); // Assumindo Product.CategoryId (PascalCase)

            // Relação Product <-> SubCategory
            modelBuilder.Entity<Product>()
                .HasOne(p => p.SubCategory)
                .WithMany(sc => sc.Products)
                .HasForeignKey(p => p.SubCategoryId); // Assumindo Product.SubCategoryId (PascalCase)

            // Relação Product <-> Provider
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Provider)
                .WithMany(pr => pr.Products)
                .HasForeignKey(p => p.ProviderId); // Assumindo Product.ProviderId (PascalCase)

            // Relação SubCategory <-> Category
            modelBuilder.Entity<SubCategory>()
                .HasOne(sc => sc.ParentCategory)
                .WithMany(/* c.SubCategories? */)
                .HasForeignKey(sc => sc.ParentCategoryId); // Assumindo SubCategory.ParentCategoryId (PascalCase)

            // --- CONFIGURAÇÃO DA ENTIDADE Brand (CORRETA) ---
            modelBuilder.Entity<Brand>(entity =>
            {
                entity.ToTable("brand"); // Nome da tabela minúsculo
                entity.Property(b => b.id); // Propriedade C# minúscula
                entity.Property(b => b.name).IsRequired().HasMaxLength(32); // Propriedade C# minúscula
                entity.Property(b => b.brand_image_url).IsRequired().HasMaxLength(512); // Propriedade C# snake_case
            });

            // --- RELAÇÃO Product -> Brand (ADICIONADA DE VOLTA) ---
            modelBuilder.Entity<Product>()
                .HasOne(p => p.Brand) // Propriedade de navegação em Product (PascalCase)
                .WithMany(b => b.Products) // Propriedade de coleção em Brand (PascalCase)
                .HasForeignKey(p => p.BrandId); // Propriedade FK em Product (PascalCase)
                // O EF Core usará a PK 'id' (minúscula) de Brand e a FK 'BrandId' (PascalCase) de Product
                // para criar a coluna FK no banco. Se você NÃO usar a convenção global de nomenclatura,
                // você precisaria mapear 'Product.BrandId' para 'brand_id' manualmente:
                // modelBuilder.Entity<Product>().Property(p => p.BrandId).HasColumnName("brand_id");
        }
    }
}

