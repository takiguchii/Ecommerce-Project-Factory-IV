using Ecommerce.Data.Context;
using Ecommerce.Entity;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Data.Seed;

public static class SeedData
{
    public static void Initialize(EcommerceDbContext context)
    {
        // Garante que o banco de dados e as tabelas foram criados
        context.Database.EnsureCreated();

        // Verifica se já existe algum produto no banco. Se sim, não faz nada.
        if (context.Products.Any())
        {
            return;
        }

        // 1. Criar Fornecedor, Categorias e SubCategorias
        var techMartProvider = new Provider 
        { 
            Name = "TechMart Distribuição", 
            Cnpj = "12345678000199",
            Email = "contato@techmart.com",
            PhoneNumber = "11999998888",
            Address = "Rua da Tecnologia, 123, São Paulo, SP"
        };

        var hardware = new Category { Name = "Hardware" };
        var perifericos = new Category { Name = "Periféricos" };
        var computadores = new Category { Name = "Computadores" };

        var processadores = new SubCategory { Name = "Processadores", ParentCategory = hardware };
        var placasDeVideo = new SubCategory { Name = "Placas de Vídeo", ParentCategory = hardware };
        var teclados = new SubCategory { Name = "Teclados", ParentCategory = perifericos };
        var mouses = new SubCategory { Name = "Mouses", ParentCategory = perifericos };
        var monitores = new SubCategory { Name = "Monitores", ParentCategory = perifericos };
        var notebooks = new SubCategory { Name = "Notebooks", ParentCategory = computadores };

        context.Providers.Add(techMartProvider);
        context.Categories.AddRange(hardware, perifericos, computadores);
        context.SubCategories.AddRange(processadores, placasDeVideo, teclados, mouses, monitores, notebooks);

        // Salva as entidades auxiliares para que o banco gere seus IDs
        context.SaveChanges();

        // 2. Criar a lista de Produtos
        var products = new List<Product>
        {
            new Product 
            { 
                Name = "Processador Core i9-13900K", 
                OriginalPrice = 3899.99m, 
                Description = "Processador de alta performance para desktops, com 24 núcleos e 32 threads.", 
                TechnicalInfo = "Socket LGA1700, Frequência Turbo Max 5.8 GHz, Gráficos UHD Intel 770",
                RawDescription = "Processador de alta performance para desktops, com 24 núcleos e 32 threads.", 
                RawTechnicalInfo = "Socket LGA1700, Frequência Turbo Max 5.8 GHz, Gráficos UHD Intel 770",
                ImageUrl = "https://i.ibb.co/bF4r2y9/cpu.png",
                Category = hardware, SubCategory = processadores, Provider = techMartProvider 
            },
            new Product 
            { 
                Name = "Placa de Vídeo RTX 4080", 
                OriginalPrice = 7999.90m, 
                Description = "Placa de vídeo com arquitetura Ada Lovelace, ideal para jogos em 4K.", 
                TechnicalInfo = "16GB GDDR6X, DLSS 3, Ray Tracing de 3ª Geração",
                RawDescription = "Placa de vídeo com arquitetura Ada Lovelace, ideal para jogos em 4K.",
                RawTechnicalInfo = "16GB GDDR6X, DLSS 3, Ray Tracing de 3ª Geração",
                ImageUrl = "https://i.ibb.co/P9pLwSg/gpu.png",
                Category = hardware, SubCategory = placasDeVideo, Provider = techMartProvider 
            },
            new Product 
            { 
                Name = "Teclado Mecânico Gamer K70 RGB", 
                OriginalPrice = 899.90m, 
                Description = "Teclado mecânico com switches Cherry MX Red e iluminação RGB customizável.", 
                TechnicalInfo = "Layout ABNT2, Estrutura em Alumínio, Descanso de pulso removível",
                RawDescription = "Teclado mecânico com switches Cherry MX Red e iluminação RGB customizável.",
                RawTechnicalInfo = "Layout ABNT2, Estrutura em Alumínio, Descanso de pulso removível",
                ImageUrl = "https://i.ibb.co/tZv3xW9/keyboard.png",
                Category = perifericos, SubCategory = teclados, Provider = techMartProvider 
            },
            new Product 
            { 
                Name = "Mouse Gamer G502 Hero", 
                OriginalPrice = 349.99m, 
                Description = "Mouse de alta precisão com sensor HERO 25K e 11 botões programáveis.", 
                TechnicalInfo = "25.600 DPI, Iluminação LIGHTSYNC RGB, Sistema de pesos ajustável",
                RawDescription = "Mouse de alta precisão com sensor HERO 25K e 11 botões programáveis.",
                RawTechnicalInfo = "25.600 DPI, Iluminação LIGHTSYNC RGB, Sistema de pesos ajustável",
                ImageUrl = "https://i.ibb.co/gDFtV23/mouse.png",
                Category = perifericos, SubCategory = mouses, Provider = techMartProvider 
            },
            new Product 
            { 
                Name = "Monitor Gamer UltraWide 34\" 144Hz", 
                OriginalPrice = 2899.00m, 
                Description = "Monitor curvo com resolução QHD para uma imersão total nos jogos.", 
                TechnicalInfo = "Resolução 3440x1440, Tempo de resposta 1ms, HDR 400",
                RawDescription = "Monitor curvo com resolução QHD para uma imersão total nos jogos.",
                RawTechnicalInfo = "Resolução 3440x1440, Tempo de resposta 1ms, HDR 400",
                ImageUrl = "https://i.ibb.co/f4c8v2x/monitor.png",
                Category = perifericos, SubCategory = monitores, Provider = techMartProvider 
            },
            new Product 
            { 
                Name = "Notebook Gamer Predator Helios", 
                OriginalPrice = 9899.50m, 
                Description = "Notebook potente para rodar os últimos lançamentos com alta performance.", 
                TechnicalInfo = "Core i7, RTX 4060, 16GB RAM DDR5, SSD 1TB NVMe, Tela 16\" QHD 165Hz",
                RawDescription = "Notebook potente para rodar os últimos lançamentos com alta performance.",
                RawTechnicalInfo = "Core i7, RTX 4060, 16GB RAM DDR5, SSD 1TB NVMe, Tela 16\" QHD 165Hz",
                ImageUrl = "https://i.ibb.co/hX5N2Vf/notebook.png",
                Category = computadores, SubCategory = notebooks, Provider = techMartProvider 
            },
            new Product 
            { 
                Name = "Processador Ryzen 7 7800X3D", 
                OriginalPrice = 2799.00m, 
                Description = "O melhor processador para jogos com tecnologia 3D V-Cache.", 
                TechnicalInfo = "Socket AM5, 8 Núcleos, 16 Threads, Frequência Max 5.0 GHz",
                RawDescription = "O melhor processador para jogos com tecnologia 3D V-Cache.",
                RawTechnicalInfo = "Socket AM5, 8 Núcleos, 16 Threads, Frequência Max 5.0 GHz",
                ImageUrl = "https://i.ibb.co/bF4r2y9/cpu.png",
                Category = hardware, SubCategory = processadores, Provider = techMartProvider 
            },
            new Product 
            { 
                Name = "Placa de Vídeo RX 7900 XTX", 
                OriginalPrice = 6999.90m, 
                Description = "Performance incrível com a arquitetura RDNA 3 e 24GB de memória.", 
                TechnicalInfo = "24GB GDDR6, FSR, Ray Tracing de 2ª Geração",
                RawDescription = "Performance incrível com a arquitetura RDNA 3 e 24GB de memória.",
                RawTechnicalInfo = "24GB GDDR6, FSR, Ray Tracing de 2ª Geração",
                ImageUrl = "https://i.ibb.co/P9pLwSg/gpu.png",
                Category = hardware, SubCategory = placasDeVideo, Provider = techMartProvider 
            },
            new Product 
            { 
                Name = "Mouse Sem Fio G Pro X Superlight", 
                OriginalPrice = 699.90m, 
                Description = "Mouse ultraleve projetado para performance profissional em e-sports.", 
                TechnicalInfo = "Menos de 63g, Sensor HERO 25K, Tecnologia sem fio LIGHTSPEED",
                RawDescription = "Mouse ultraleve projetado para performance profissional em e-sports.",
                RawTechnicalInfo = "Menos de 63g, Sensor HERO 25K, Tecnologia sem fio LIGHTSPEED",
                ImageUrl = "https://i.ibb.co/gDFtV23/mouse.png",
                Category = perifericos, SubCategory = mouses, Provider = techMartProvider 
            },
            new Product 
            { 
                Name = "Notebook Swift Go 14", 
                OriginalPrice = 5299.00m, 
                Description = "Notebook ultrafino e leve com tela OLED, ideal para trabalho e estudos.", 
                TechnicalInfo = "Core i5, 16GB RAM LPDDR5, SSD 512GB NVMe, Tela 14\" 2.8K OLED",
                RawDescription = "Notebook ultrafino e leve com tela OLED, ideal para trabalho e estudos.",
                RawTechnicalInfo = "Core i5, 16GB RAM LPDDR5, SSD 512GB NVMe, Tela 14\" 2.8K OLED",
                ImageUrl = "https://i.ibb.co/hX5N2Vf/notebook.png",
                Category = computadores, SubCategory = notebooks, Provider = techMartProvider 
            }
        };

        context.Products.AddRange(products);
        
        // 3. Salvar tudo no banco de dados
        context.SaveChanges();
    }
}