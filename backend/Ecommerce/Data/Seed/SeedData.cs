using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ecommerce.Data.Context;
using Ecommerce.Entity;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Ecommerce.Data.Seed;

// 1. DTO 100% alinhado com o JSON final
public class ProductJsonDto
{
    [JsonPropertyName("code")]
    public string Code { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("brand")]
    public string BrandName { get; set; }
    
    [JsonPropertyName("brand_image")]
    public string BrandImage { get; set; }

    [JsonPropertyName("category")]
    public string CategoryName { get; set; }

    [JsonPropertyName("sub_category")]
    public string SubCategoryName { get; set; }

    [JsonPropertyName("original_price")]
    public string OriginalPrice { get; set; }

    [JsonPropertyName("discount_price")]
    public string DiscountPrice { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
    
    [JsonPropertyName("technical_info")]
    public string TechnicalInfo { get; set; }

    [JsonPropertyName("raw_description")]
    public string RawDescription { get; set; }

    [JsonPropertyName("raw_technical_info")]
    public string RawTechnicalInfo { get; set; }
    
    [JsonPropertyName("image_url0")]
    public string CoverImageUrl { get; set; }

    [JsonPropertyName("image_url1")] 
    public string AdditionalImageUrl1 { get; set; }

    [JsonPropertyName("image_url2")] 
    public string AdditionalImageUrl2 { get; set; }

    [JsonPropertyName("image_url3")] 
    public string AdditionalImageUrl3 { get; set; }
    
    [JsonPropertyName("image_url4")]
    public string AdditionalImageUrl4 { get; set; }

    [JsonPropertyName("stars")]
    public decimal AverageStars { get; set; }

    [JsonPropertyName("rating")]
    public string ReviewCount { get; set; }
}


public static class SeedData
{
    public static void Initialize(EcommerceDbContext context)
    {
        context.Database.EnsureCreated();

        // Se já existirem produtos, não faz nada.
        if (context.Products.Any())
        {
            return;
        }

        // Seed inicial de entidades base (roda apenas uma vez no banco vazio)
        if (!context.Categories.Any())
        {
            var techMartProvider = new Provider
            {
                Name = "TechMart Distribuição", Cnpj = "12345678000199", Email = "contato@techmart.com",
                PhoneNumber = "11999998888", Address = "Rua da Tecnologia, 123, São Paulo, SP"
            };
            context.Providers.Add(techMartProvider);

            var hardware = new Category { Name = "Hardware", ImageUrlCategory = "https://www.kabum.com.br/_next/image?url=https%3A%2F%2Fstatic.kabum.com.br%2Fconteudo%2Fcategorias%2FHARDWARE_1700588665.png&w=256&q=75" };
            context.Categories.Add(hardware);

            var ssd = new SubCategory { Name = "SSD", ParentCategory = hardware };
            context.SubCategories.Add(ssd);
            
            context.SaveChanges();
        }
        
        var allProductsToSeed = new List<Product>();
        var jsonDataPath = Path.Combine(AppContext.BaseDirectory, "Data", "Seed", "JsonData");
        
        var defaultProvider = context.Providers.First();

        if (Directory.Exists(jsonDataPath))
        {
            var jsonFiles = Directory.GetFiles(jsonDataPath, "*.json");

            foreach (var file in jsonFiles)
            {
                var jsonString = File.ReadAllText(file);
                var productDtos = JsonSerializer.Deserialize<List<ProductJsonDto>>(jsonString);

                if (productDtos == null || !productDtos.Any()) continue;

                foreach (var dto in productDtos)
                {
                    // 2. Lógica para garantir que a marca exista ("Get or Create")
                    var brand = context.Brands.FirstOrDefault(b => b.Name == dto.BrandName);
                    if (brand == null)
                    {
                        brand = new Brand { Name = dto.BrandName, ImageUrl = NormalizeImageUrl(dto.BrandImage) };
                        context.Brands.Add(brand);
                    }

                    // 3. Encontra categoria e subcategoria
                    var cleanCategoryName = dto.CategoryName.Replace(">", "").Trim();
                    var cleanSubCategoryName = dto.SubCategoryName.Replace(">", "").Trim();
                    var category = context.Categories.FirstOrDefault(c => c.Name == cleanCategoryName);
                    var subCategory = context.SubCategories.FirstOrDefault(sc => sc.Name == cleanSubCategoryName);

                    if (category == null || subCategory == null)
                    {
                        Console.WriteLine($"AVISO: Categoria '{cleanCategoryName}' ou Subcategoria '{cleanSubCategoryName}' não encontrada para '{dto.Name}'. Pulando.");
                        continue;
                    }

                    // 4. Mapeamento final, incluindo os novos campos e a limpeza de dados
                    allProductsToSeed.Add(new Product
                    {
                        Code = dto.Code,
                        Name = dto.Name,
                        OriginalPrice = ParsePrice(dto.OriginalPrice) ?? 0m,
                        DiscountPrice = ParsePrice(dto.DiscountPrice),
                        Description = dto.Description,
                        TechnicalInfo = dto.TechnicalInfo,
                        RawDescription = dto.RawDescription,
                        RawTechnicalInfo = dto.RawTechnicalInfo,
                        CoverImageUrl = NormalizeImageUrl(dto.CoverImageUrl),
                        AdditionalImageUrl1 = NormalizeImageUrl(dto.AdditionalImageUrl1), 
                        AdditionalImageUrl2 = NormalizeImageUrl(dto.AdditionalImageUrl2), 
                        AdditionalImageUrl3 = NormalizeImageUrl(dto.AdditionalImageUrl3),
                        AdditionalImageUrl4 = NormalizeImageUrl(dto.AdditionalImageUrl4),
                        AverageStars = dto.AverageStars,
                        ReviewCount = ParseReviewCount(dto.ReviewCount),
                        Category = category,
                        SubCategory = subCategory,
                        Brand = brand,
                        Provider = defaultProvider
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
        var cleanString = Regex.Replace(priceString, @"[R$\s]", "").Trim();
        if (decimal.TryParse(cleanString, NumberStyles.Currency, culture, out decimal price))
        {
            return price;
        }
        return null;
    }
    
    // 5. Novo método auxiliar para limpar e converter o número de avaliações
    private static int ParseReviewCount(string reviewString)
    {
        if (string.IsNullOrWhiteSpace(reviewString)) return 0;
        var cleanString = Regex.Replace(reviewString, @"[()]", "").Trim();
        if (int.TryParse(cleanString, out int count))
        {
            return count;
        }
        return 0;
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