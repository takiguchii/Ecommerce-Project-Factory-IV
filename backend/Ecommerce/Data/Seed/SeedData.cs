using Ecommerce.Data.Context; 
using Ecommerce.Entity;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Data.Seed;

public static class SeedData
{
    public static void Initialize(EcommerceDbContext context)
    {
        // Verifica se já existe algum produto no banco. Se sim, não faz nada.
        if (context.Products.Any())
        {
            return; 
        }

        // 1. Criar Categorias, SubCategorias e Fornecedores primeiro
        var eletronicos = new Category { Name = "Eletrônicos" };
        var vestuario = new Category { Name = "Vestuário" };

        var celulares = new SubCategory { Name = "Celulares", ParentCategory = eletronicos };
        var camisetas = new SubCategory { Name = "Camisetas", ParentCategory = vestuario };

        var techProvider = new Provider { Name = "Tech Distribuidora", Cnpj = "11222333000144" };
        var fashionProvider = new Provider { Name = "Fashion Hub", Cnpj = "44555666000177" };
        
        context.Categories.AddRange(eletronicos, vestuario);
        context.SubCategories.AddRange(celulares, camisetas);
        context.Providers.AddRange(techProvider, fashionProvider);
        
        context.SaveChanges();

        // 2. Criar os Produtos usando as entidades 
        var products = new List<Product>
        {
            new Product 
            { 
                Name = "Smartphone Modelo X", 
                OriginalPrice = 2999.99m, 
                Description = "Um smartphone de última geração com câmera tripla.", 
                TechnicalInfo = "Tela OLED, 128GB, 8GB RAM", 
                // --- CORREÇÃO AQUI ---
                RawDescription = "Um smartphone de última geração com câmera tripla.", 
                RawTechnicalInfo = "Tela OLED, 128GB, 8GB RAM",
                Rating = 0, // Definir valores padrão
                RatingQuantity = 0,
                Category = eletronicos, 
                SubCategory = celulares, 
                Provider = techProvider 
            },
            new Product 
            { 
                Name = "Camiseta Básica Branca", 
                OriginalPrice = 79.90m, 
                Description = "Camiseta de algodão 100% orgânico.", 
                TechnicalInfo = "Algodão Pima, Fio 30.1",
                // --- CORREÇÃO AQUI ---
                RawDescription = "Camiseta de algodão 100% orgânico.",
                RawTechnicalInfo = "Algodão Pima, Fio 30.1",
                Rating = 0,
                RatingQuantity = 0,
                Category = vestuario, 
                SubCategory = camisetas, 
                Provider = fashionProvider 
            },
            // Complete os outros produtos da mesma forma...
        };
        context.Products.AddRange(products);
        
        // 3. Salvar tudo no banco de dados
        context.SaveChanges();
    }
}