using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ecommerce.Data.Context;
using Ecommerce.Entity;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Data.Seed;

// DTO para mapear a estrutura do nosso JSON padronizado com os novos nomes
public class ProductJsonDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("original_price")]
    public string OriginalPrice { get; set; }

    [JsonPropertyName("discount_price")]
    public string DiscountPrice { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
    
    [JsonPropertyName("technical_info")]
    public string TechnicalInfo { get; set; }

    // --- NOMES ATUALIZADOS AQUI ---
    [JsonPropertyName("cover_image_url")]
    public string CoverImageUrl { get; set; }

    [JsonPropertyName("additional_image_url_1")] 
    public string AdditionalImageUrl1 { get; set; }

    [JsonPropertyName("additional_image_url_2")] 
    public string AdditionalImageUrl2 { get; set; }

    [JsonPropertyName("additional_image_url_3")] 
    public string AdditionalImageUrl3 { get; set; }

    [JsonPropertyName("subcategory_name")]
    public string SubCategoryName { get; set; }
}


public static class SeedData
{
    public static void Initialize(EcommerceDbContext context)
    {
        context.Database.EnsureCreated();

        if (context.Products.Any() || context.Categories.Any())
        {
            return;
        }

        var techMartProvider = new Provider
        {
            Name = "TechMart Distribuição", Cnpj = "12345678000199", Email = "contato@techmart.com",
            PhoneNumber = "11999998888", Address = "Rua da Tecnologia, 123, São Paulo, SP"
        };
        context.Providers.Add(techMartProvider);

        var hardware = new Category { Name = "Hardware", ImageUrlCategory = "https://www.kabum.com.br/_next/image?url=https%3A%2F%2Fstatic.kabum.com.br%2Fconteudo%2Fcategorias%2FHARDWARE_1700588665.png&w=256&q=75" };
        var perifericos = new Category { Name = "Periféricos", ImageUrlCategory = "https://www.kabum.com.br/_next/image?url=https%3A%2F%2Fstatic.kabum.com.br%2Fconteudo%2Fcategorias%2FPERIFERICOS_1700588652.png&w=256&q=75" };
        var computadores = new Category { Name = "Computadores", ImageUrlCategory = "https://www.kabum.com.br/_next/image?url=https%3A%2F%2Fstatic.kabum.com.br%2Fconteudo%2Fcategorias%2FCOMPUTADORES_1731081639.png&w=256&q=75" };
        var videoGames = new Category { Name = "Video Games", ImageUrlCategory = "https://www.kabum.com.br/_next/image?url=https%3A%2F%2Fstatic.kabum.com.br%2Fconteudo%2Fcategorias%2FGAMER_1700588706.png&w=256&q=75" };
        var celulares = new Category { Name = "Celular & Smartphones", ImageUrlCategory = "https://www.kabum.com.br/_next/image?url=https%3A%2F%2Fstatic.kabum.com.br%2Fconteudo%2Fcategorias%2FCELULAR-SMARTPHONE_1731081407.png&w=256&q=75" };
        var tv = new Category { Name = "TV", ImageUrlCategory = "https://www.kabum.com.br/_next/image?url=https%3A%2F%2Fstatic.kabum.com.br%2Fconteudo%2Fcategorias%2FTV_1700588559.png&w=256&q=75" };
        var audio = new Category { Name = "Áudio", ImageUrlCategory = "https://www.kabum.com.br/_next/image?url=https%3A%2F%2Fstatic.kabum.com.br%2Fconteudo%2Fcategorias%2FAUDIO_1700588544.png&w=256&q=75" };
        var casaInteligente = new Category { Name = "Casa Inteligente", ImageUrlCategory = "https://www.kabum.com.br/_next/image?url=https%3A%2F%2Fstatic.kabum.com.br%2Fconteudo%2Fcategorias%2FCASA-INTELIGENTE_1731081391.png&w=256&q=75" };
        
        context.Categories.AddRange(hardware, perifericos, computadores, videoGames, celulares, tv, audio, casaInteligente);

        var placasDeVideo = new SubCategory { Name = "Placas de Vídeo", ParentCategory = hardware };
        var notebooks = new SubCategory { Name = "Notebooks", ParentCategory = computadores };
        var mouses = new SubCategory { Name = "Mouses", ParentCategory = perifericos };
        var headsets = new SubCategory { Name = "Headsets", ParentCategory = perifericos };
        var consoles = new SubCategory { Name = "Consoles", ParentCategory = videoGames };
        var acessoriosGamer = new SubCategory { Name = "Acessórios Gamer", ParentCategory = perifericos };
        var monitores = new SubCategory { Name = "Monitores", ParentCategory = perifericos };
        var smartphones = new SubCategory { Name = "Smartphones", ParentCategory = celulares };
        var smartTvs = new SubCategory { Name = "Smart TVs", ParentCategory = tv };
        var fonesDeOuvido = new SubCategory { Name = "Fones de Ouvido", ParentCategory = audio };
        var assistentesVirtuais = new SubCategory { Name = "Assistentes Virtuais", ParentCategory = casaInteligente };
        var placasMae = new SubCategory { Name = "Placas-Mãe", ParentCategory = hardware };
        var armazenamento = new SubCategory { Name = "Armazenamento", ParentCategory = hardware };

        context.SubCategories.AddRange(placasDeVideo, notebooks, mouses, headsets, consoles, acessoriosGamer, monitores, smartphones, smartTvs, fonesDeOuvido, assistentesVirtuais, placasMae, armazenamento);
        
        context.SaveChanges();
        
        var allProductsToSeed = new List<Product>();
        var jsonDataPath = Path.Combine(AppContext.BaseDirectory, "Data", "Seed", "JsonData");

        if (Directory.Exists(jsonDataPath))
        {
            var jsonFiles = Directory.GetFiles(jsonDataPath, "*.json");

            foreach (var file in jsonFiles)
            {
                var jsonString = File.ReadAllText(file);
                var productDtos = JsonSerializer.Deserialize<List<ProductJsonDto>>(jsonString);

                if (productDtos == null || !productDtos.Any()) continue;
                
                var subCategoryName = productDtos.First().SubCategoryName;
                var subCategory = context.SubCategories.Include(sc => sc.ParentCategory)
                                           .FirstOrDefault(sc => sc.Name == subCategoryName);

                if (subCategory == null)
                {
                    Console.WriteLine($"AVISO: Subcategoria '{subCategoryName}' do arquivo '{Path.GetFileName(file)}' não encontrada. Pulando produtos.");
                    continue;
                }

                foreach (var dto in productDtos)
                {
                    allProductsToSeed.Add(new Product
                    {
                        Name = dto.Name,
                        OriginalPrice = ParsePrice(dto.OriginalPrice) ?? 0m,
                        DiscountPrice = ParsePrice(dto.DiscountPrice),
                        Description = dto.Description,
                        TechnicalInfo = dto.TechnicalInfo,
                        RawDescription = dto.Description,
                        RawTechnicalInfo = dto.TechnicalInfo,
                        
                        CoverImageUrl = NormalizeImageUrl(dto.CoverImageUrl),
                        AdditionalImageUrl1 = NormalizeImageUrl(dto.AdditionalImageUrl1), 
                        AdditionalImageUrl2 = NormalizeImageUrl(dto.AdditionalImageUrl2), 
                        AdditionalImageUrl3 = NormalizeImageUrl(dto.AdditionalImageUrl3),
                        
                        AverageStars = 0,
                        ReviewCount = 0,
                        
                        Category = subCategory.ParentCategory,
                        SubCategory = subCategory,
                        Provider = techMartProvider
                    });
                }
            }
        }
        
        if(allProductsToSeed.Any())
        {
            context.Products.AddRange(allProductsToSeed);
            context.SaveChanges();
        }
    }

    private static decimal? ParsePrice(string priceString)
    {
        if (string.IsNullOrWhiteSpace(priceString)) return null;
        var culture = new CultureInfo("pt-BR");
        var cleanString = priceString.Replace("R$", "").Trim();
        if (decimal.TryParse(cleanString, NumberStyles.Currency, culture, out decimal price))
        {
            return price;
        }
        return null;
    }

    private static string NormalizeImageUrl(string endpointImage)
    {
        if (string.IsNullOrEmpty(endpointImage))
        {
            return "";
        }

        if (endpointImage.StartsWith("http"))
        {
            return endpointImage;
        }

        if (endpointImage.StartsWith("/_next/image"))
        {
            return "https://www.kabum.com.br" + endpointImage;
        }

        if (endpointImage.StartsWith("/produtos/fotos"))
        {
            return "https://images.kabum.com.br" + endpointImage;
        }

        return endpointImage;
    }
}