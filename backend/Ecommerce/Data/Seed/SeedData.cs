using Ecommerce.Data.Context;
using Ecommerce.Entity;

namespace Ecommerce.Data.Seed;

public static class SeedData
{
    public static void Initialize(EcommerceDbContext context)
    {
        context.Database.EnsureCreated();

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

        var hardware = new Category { Name = "Hardware", ImageUrlCategory = "https://www.kabum.com.br/_next/image?url=https%3A%2F%2Fstatic.kabum.com.br%2Fconteudo%2Fcategorias%2FHARDWARE_1700588665.png&w=256&q=75" };
        var perifericos = new Category { Name = "Periféricos", ImageUrlCategory = "https://www.kabum.com.br/_next/image?url=https%3A%2F%2Fstatic.kabum.com.br%2Fconteudo%2Fcategorias%2FPERIFERICOS_1700588652.png&w=256&q=75" };
        var computadores = new Category { Name = "Computadores", ImageUrlCategory = "https://www.kabum.com.br/_next/image?url=https%3A%2F%2Fstatic.kabum.com.br%2Fconteudo%2Fcategorias%2FCOMPUTADORES_1731081639.png&w=256&q=75" };
        var videoGames = new Category { Name = "Video Games", ImageUrlCategory = "https://www.kabum.com.br/_next/image?url=https%3A%2F%2Fstatic.kabum.com.br%2Fconteudo%2Fcategorias%2FGAMER_1700588706.png&w=256&q=75" };

        var placasDeVideo = new SubCategory { Name = "Placas de Vídeo", ParentCategory = hardware };
        var notebooks = new SubCategory { Name = "Notebooks", ParentCategory = computadores };
        var mouses = new SubCategory { Name = "Mouses", ParentCategory = perifericos };
        var headsets = new SubCategory { Name = "Headsets", ParentCategory = perifericos };
        var consoles = new SubCategory { Name = "Consoles", ParentCategory = videoGames };
        var acessoriosGamer = new SubCategory { Name = "Acessórios Gamer", ParentCategory = perifericos };
        var monitores = new SubCategory { Name = "Monitores", ParentCategory = perifericos };

        context.Providers.Add(techMartProvider);
        context.Categories.AddRange(hardware, perifericos, computadores, videoGames);
        context.SubCategories.AddRange(placasDeVideo, notebooks, mouses, headsets, consoles, acessoriosGamer, monitores);
        context.SaveChanges();

        // 2. Criar a lista de Produtos com base nos seus links
        var products = new List<Product>
        {
            new Product
            {
                Name = "Placa de Vídeo ASRock RX 6600 Challenger White", OriginalPrice = 1399.99m, DiscountPrice = 1299.99m,
                Description = "Placa de vídeo com design branco elegante para jogos em Full HD, com 8GB de memória GDDR6 e arquitetura RDNA 2.",
                TechnicalInfo = "8GB GDDR6, DirectX 12 Ultimate, Interface 128-bit",
                RawDescription = "Uma placa de vídeo robusta para jogos em Full HD, com design branco elegante e 8GB de memória GDDR6.",
                RawTechnicalInfo = "8GB GDDR6, Arquitetura RDNA 2, DirectX 12 Ultimate, Interface de Memória 128-bit",
                ImageUrl = "https://www.kabum.com.br/_next/image?url=https%3A%2F%2Fimages7.kabum.com.br%2Fprodutos%2Ffotos%2F695107%2Fplaca-de-video-asrock-rx-6600-challenger-white-amd-radeon-8gb-gddr6-directx-12-ultimate-rdna-2-90-ga4uzz-00uanf_1742841360_gg.jpg&w=640&q=75",
                Rating = 5, RatingQuantity = 92, Category = hardware, SubCategory = placasDeVideo, Provider = techMartProvider
            },
            new Product
            {
                Name = "Console Sony Playstation 5 825GB Edição Digital", OriginalPrice = 3799.90m,
                Description = "Experimente o carregamento ultrarrápido com o SSD de altíssima velocidade e uma geração totalmente nova de jogos.",
                TechnicalInfo = "CPU x86-64-AMD Ryzen Zen 2, 8 Cores, GPU AMD Radeon RDNA 2, Memória 16GB GDDR6",
                RawDescription = "Experimente o carregamento ultrarrápido com o SSD de altíssima velocidade, imersão mais profunda e uma geração totalmente nova de jogos.",
                RawTechnicalInfo = "CPU x86-64-AMD Ryzen Zen 2, 8 Cores, GPU AMD Radeon RDNA 2, Memória 16GB GDDR6",
                ImageUrl = "https://www.kabum.com.br/_next/image?url=https%3A%2F%2Fimages2.kabum.com.br%2Fprodutos%2Ffotos%2F922662%2Fconsole-sony-playstation-5-825gb-astro-bot-e-gran-turismo-edicao-digital-1000050614_1758119259_gg.jpg&w=640&q=75",
                Rating = 5, RatingQuantity = 1250, Category = videoGames, SubCategory = consoles, Provider = techMartProvider
            },
            new Product
            {
                Name = "Notebook Gamer Acer Nitro V, Intel Core i5 13ª Gen", OriginalPrice = 4599.99m, DiscountPrice = 4399.99m,
                Description = "Performance de última geração com processador Intel Core i5 de 13ª Geração e placa de vídeo NVIDIA GeForce RTX 4050.",
                TechnicalInfo = "8GB RAM DDR5, SSD 512GB NVMe, RTX 4050 6GB, Tela 15.6\" Full HD 144Hz",
                RawDescription = "Performance de última geração para seus jogos com processador Intel Core i5 de 13ª Geração e placa de vídeo NVIDIA GeForce RTX 4050.",
                RawTechnicalInfo = "8GB RAM DDR5, SSD 512GB NVMe, RTX 4050 6GB, Tela 15.6\" Full HD 144Hz",
                ImageUrl = "https://www.kabum.com.br/_next/image?url=https%3A%2F%2Fimages2.kabum.com.br%2Fprodutos%2Ffotos%2Fsync_mirakl%2F590662%2Fxlarge%2FNotebook-Gamer-Acer-Nitro-V-Intel-Core-i5-13-Gen-8GB-SSD-512GB-RTX4050-Tela-15-6-Full-HD-Windows-11-Home-Anv15-51-54dl_1755786291.jpg&w=640&q=75",
                Rating = 4, RatingQuantity = 180, Category = computadores, SubCategory = notebooks, Provider = techMartProvider
            },
            new Product
            {
                Name = "Mouse Gamer Redragon Cobra Chroma RGB 10000DPI", OriginalPrice = 119.99m,
                Description = "Mouse ergonômico com 7 botões programáveis, iluminação RGB e sensor de alta precisão para máxima performance.",
                TechnicalInfo = "Sensor PMW3325, 10000 DPI, Iluminação Chroma RGB, Software para customização",
                RawDescription = "Mouse ergonômico com 7 botões programáveis, iluminação RGB e sensor de alta precisão para máxima performance em jogos.",
                RawTechnicalInfo = "Sensor PMW325, 10000 DPI, Iluminação Chroma RGB, Software para customização",
                ImageUrl = "https://www.kabum.com.br/_next/image?url=https%3A%2F%2Fimages5.kabum.com.br%2Fprodutos%2Ffotos%2F94555%2Fmouse-gamer-redragon-cobra-chroma-rgb-10000dpi-7-botoes-preto-m711-v2_1742821619_gg.jpg&w=640&q=75",
                Rating = 5, RatingQuantity = 8897, Category = perifericos, SubCategory = mouses, Provider = techMartProvider
            },
            new Product
            {
                Name = "Headset Gamer HyperX Cloud Stinger 2", OriginalPrice = 249.90m, DiscountPrice = 219.90m,
                Description = "Leveza e conforto com áudio imersivo. Drivers de 50mm e microfone com cancelamento de ruído.",
                TechnicalInfo = "Drivers de 50mm, DTS Headphone:X Spatial Audio, Controles de áudio no fone, Microfone flexível",
                RawDescription = "Leveza e conforto com áudio imersivo. Drivers de 50mm e microfone com cancelamento de ruído.",
                RawTechnicalInfo = "Drivers de 50mm, DTS Headphone:X Spatial Audio, Controles de áudio no fone, Microfone flexível",
                ImageUrl = "https://www.kabum.com.br/_next/image?url=https%3A%2F%2Fimages0.kabum.com.br%2Fprodutos%2Ffotos%2F461160%2Fheadset-gamer-hyperx-cloud-stinger-2-drivers-50mm-preto-519t1aa_1689972862_gg.jpg&w=640&q=75",
                Rating = 4, RatingQuantity = 450, Category = perifericos, SubCategory = headsets, Provider = techMartProvider
            },
            new Product
            {
                Name = "Notebook Acer Aspire 5, AMD Ryzen 7 5700U", OriginalPrice = 3299.00m,
                Description = "Ideal para trabalho e produtividade, com processador de 8 núcleos, 16GB de RAM e design fino e leve.",
                TechnicalInfo = "16GB RAM DDR4, SSD 512GB NVMe, Tela 15.6\" Full HD IPS, AMD Radeon Graphics",
                RawDescription = "Ideal para trabalho e produtividade, com processador de 8 núcleos, 16GB de RAM e design fino e leve.",
                RawTechnicalInfo = "16GB RAM DDR4, SSD 512GB NVMe, Tela 15.6\" Full HD IPS, AMD Radeon Graphics",
                ImageUrl = "https://www.kabum.com.br/_next/image?url=https%3A%2F%2Fimages5.kabum.com.br%2Fprodutos%2Ffotos%2F564915%2Fnotebook-acer-aspire-5-amd-ryzen7-5700u-16gb-ram-ssd-512gb-15-6-full-hd-ips-amd-radeon-linux-prata-a515-45-r74n_1716491063_gg.jpg&w=640&q=75",
                Rating = 5, RatingQuantity = 123, Category = computadores, SubCategory = notebooks, Provider = techMartProvider
            },
            new Product
            {
                Name = "Headset Gamer Havit, Drivers 53mm", OriginalPrice = 189.99m,
                Description = "Headset com som surround, microfone plug-and-play e compatibilidade multiplataforma.",
                TechnicalInfo = "Drivers de 53mm, Plug 3.5mm, Design over-ear, Haste de metal ajustável",
                RawDescription = "Headset com som surround, microfone plug-and-play e compatibilidade multiplataforma.",
                RawTechnicalInfo = "Drivers de 53mm, Plug 3.5mm, Design over-ear, Haste de metal ajustável",
                ImageUrl = "https://www.kabum.com.br/_next/image?url=https%3A%2F%2Fimages0.kabum.com.br%2Fprodutos%2Ffotos%2F102770%2Fheadset-gamer-havit-drivers-53mm-hv-h2002d_headset-gamer-havit-drivers-53mm-hv-h2002d_1564056874_gg.jpg&w=640&q=75",
                Rating = 4, RatingQuantity = 1302, Category = perifericos, SubCategory = headsets, Provider = techMartProvider
            },
            new Product
            {
                Name = "Mouse Gamer Sem Fio Logitech G305 Lightspeed", OriginalPrice = 229.90m, DiscountPrice = 199.90m,
                Description = "Liberdade sem fios com a tecnologia Lightspeed, sensor HERO de alta performance e longa duração de bateria.",
                TechnicalInfo = "Sensor HERO 12.000 DPI, 6 Botões Programáveis, Até 250h de autonomia com uma pilha AA",
                RawDescription = "Liberdade sem fios com a tecnologia Lightspeed, sensor HERO de alta performance e longa duração de bateria.",
                RawTechnicalInfo = "Sensor HERO 12.000 DPI, 6 Botões Programáveis, Até 250h de autonomia com uma pilha AA",
                ImageUrl = "https://www.kabum.com.br/_next/image?url=https%3A%2F%2Fimages8.kabum.com.br%2Fprodutos%2Ffotos%2F269638%2Fmouse-gamer-sem-fio-logitech-g305-lightspeed-12-000-dpi-6-botoes-programaveis-branco-910-005290_1755626227_gg.jpg&w=640&q=75",
                Rating = 5, RatingQuantity = 2348, Category = perifericos, SubCategory = mouses, Provider = techMartProvider
            },
            new Product
            {
                Name = "Volante Logitech G29 Driving Force", OriginalPrice = 1799.99m,
                Description = "Sinta cada derrapagem e curva com o Force Feedback realista. Volante para PS5, PS4, PS3 e PC.",
                TechnicalInfo = "Rotação de 900 graus, Force Feedback de motor duplo, Pedais de freio, acelerador e embreagem",
                RawDescription = "Sinta cada derrapagem e curva com o Force Feedback realista. Volante para PS5, PS4, PS3 e PC.",
                RawTechnicalInfo = "Rotação de 900 graus, Force Feedback de motor duplo, Pedais de freio, acelerador e embreagem",
                ImageUrl = "https://www.kabum.com.br/_next/image?url=https%3A%2F%2Fimages5.kabum.com.br%2Fprodutos%2Ffotos%2F69305%2Fvolante-logitech-g29-driving-force-para-ps5-ps4-ps3-e-pc-941-000111_1644503634_gg.jpg&w=640&q=75",
                Rating = 5, RatingQuantity = 988, Category = perifericos, SubCategory = acessoriosGamer, Provider = techMartProvider
            },
            new Product
            {
                Name = "Monitor Gamer LG UltraGear 24\" LED 180Hz", OriginalPrice = 849.99m,
                Description = "Monitor de alta performance com taxa de atualização de 180Hz e 1ms de tempo de resposta para jogabilidade ultra fluida.",
                TechnicalInfo = "Tela de 24\" Full HD, 180Hz, 1ms (GtG), HDR10, AMD FreeSync",
                RawDescription = "Monitor de alta performance com taxa de atualização de 180Hz e 1ms de tempo de resposta para uma jogabilidade ultra fluida.",
                RawTechnicalInfo = "Tela de 24\" Full HD, 180Hz, 1ms (GtG), HDR10, AMD FreeSync",
                ImageUrl = "https://www.kabum.com.br/_next/image?url=https%3A%2F%2Fimages5.kabum.com.br%2Fprodutos%2Ffotos%2Fsync_mirakl%2F617865%2Fxlarge%2FMonitor-Gamer-LG-Ultragear-24-LED-180Hz-1ms-HDR10-AMD-Freesync-DisplayPort-HDMI-24gs60f-b_1759239641.jpg&w=640&q=75",
                Rating = 5, RatingQuantity = 329, Category = perifericos, SubCategory = monitores, Provider = techMartProvider
            },
            new Product
            {
                Name = "Placa de Video ASRock AMD Radeon RX 6600 CLD", OriginalPrice = 1299.99m,
                Description = "Placa de vídeo com 8GB de memória GDDR6 e design de resfriamento com duas ventoinhas.",
                TechnicalInfo = "8GB GDDR6, Clock de até 2491 MHz, Suporte a 8K, PCI Express 4.0",
                RawDescription = "Placa de vídeo com 8GB de memória GDDR6 e design de resfriamento com duas ventoinhas para manter a temperatura baixa.",
                RawTechnicalInfo = "8GB GDDR6, Clock de até 2491 MHz, Suporte a 8K, PCI Express 4.0",
                ImageUrl = "https://www.kabum.com.br/_next/image?url=https%3A%2F%2Fimages4.kabum.com.br%2Fprodutos%2Ffotos%2F235984%2Fplaca-de-video-asrock-amd-radeon-rx-6600-cld-8g-8gb-90-ga2rzz-00uanf_1634738812_gg.jpg&w=640&q=75",
                Rating = 4, RatingQuantity = 765, Category = hardware, SubCategory = placasDeVideo, Provider = techMartProvider
            },
            new Product
            {
                Name = "Notebook Gamer Acer Nitro V15 Intel Core i7", OriginalPrice = 6899.99m, DiscountPrice = 6599.99m,
                Description = "Potência e velocidade com o processador Intel Core i7 de 13ª geração e placa de vídeo RTX 4050.",
                TechnicalInfo = "Intel Core i7-13620H, 16GB RAM DDR5, RTX 4050 6GB, SSD 512GB, Tela 15.6\" Full HD 144Hz",
                RawDescription = "Potência e velocidade com o processador Intel Core i7 de 13ª geração, 16GB de RAM e placa de vídeo RTX 4050.",
                RawTechnicalInfo = "Intel Core i7-13620H, 16GB RAM DDR5, RTX 4050 6GB, SSD 512GB, Tela 15.6\" Full HD 144Hz",
                ImageUrl = "https://www.kabum.com.br/_next/image?url=https%3A%2F%2Fimages2.kabum.com.br%2Fprodutos%2Ffotos%2Fsync_mirakl%2F916712%2Fxlarge%2FNotebook-Gamer-Acer-Nitro-V15-Intel-Core-i7-13620h-13-G-16GB-RAM-RTX-4050-SSD-512GB-Tela-15-6-Full-HD-Linux-Anv15-52-737p_1759245375.jpg&w=640&q=75",
                Rating = 5, RatingQuantity = 45, Category = computadores, SubCategory = notebooks, Provider = techMartProvider
            }
        };

        context.Products.AddRange(products);
        context.SaveChanges();
    }
}