using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ecommerce.Data.Context;
using Ecommerce.Entity;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Ecommerce.Data.Seed;

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

        if (context.Products.Any())
        {
            return;
        }

        if (!context.Categories.Any())
        {
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
            
            context.SaveChanges();
        }
        
        
        var allProductsToSeed = new List<Product>();
        var jsonDataPath = Path.Combine(AppContext.BaseDirectory, "Data", "Seed", "JsonData");
        
        var defaultProvider = context.Providers.First();
        
        // <<< NOSSOS FILTROS/CACHES PARA TODAS AS ENTIDADES >>>
        var processedBrands = new Dictionary<string, Brand>();
        var processedCategories = new Dictionary<string, Category>();
        var processedSubCategories = new Dictionary<string, SubCategory>();

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
                    var cleanCategoryName = dto.CategoryName.Replace(">", "").Trim();
                    var cleanSubCategoryName = dto.SubCategoryName.Replace(">", "").Trim();

                    // --- FILTRO PARA CATEGORY ---
                    Category category;
                    if (processedCategories.ContainsKey(cleanCategoryName))
                    {
                        category = processedCategories[cleanCategoryName];
                    }
                    else
                    {
                        category = context.Categories.FirstOrDefault(c => c.Name == cleanCategoryName);
                        if (category == null)
                        {
                            // Se a categoria do JSON não existir, criamos uma nova (sem imagem, pois o JSON não fornece)
                            category = new Category { Name = cleanCategoryName, ImageUrlCategory = "" };
                            context.Categories.Add(category);
                        }
                        processedCategories.Add(cleanCategoryName, category);
                    }
                    
                    // --- FILTRO PARA SUBCATEGORY ---
                    SubCategory subCategory;
                    if (processedSubCategories.ContainsKey(cleanSubCategoryName))
                    {
                        subCategory = processedSubCategories[cleanSubCategoryName];
                    }
                    else
                    {
                        subCategory = context.SubCategories.FirstOrDefault(sc => sc.Name == cleanSubCategoryName);
                        if (subCategory == null)
                        {
                            // Se a subcategoria não existir, criamos e associamos à categoria encontrada acima
                            subCategory = new SubCategory { Name = cleanSubCategoryName, ParentCategory = category };
                            context.SubCategories.Add(subCategory);
                        }
                        processedSubCategories.Add(cleanSubCategoryName, subCategory);
                    }

                    // --- FILTRO PARA BRAND ---
                    Brand brand;
                    if (processedBrands.ContainsKey(dto.BrandName))
                    {
                        brand = processedBrands[dto.BrandName];
                    }
                    else
                    {
                        brand = context.Brands.FirstOrDefault(b => b.Name == dto.BrandName);
                        if (brand == null)
                        {
                            brand = new Brand { Name = dto.BrandName, ImageUrl = NormalizeImageUrl(dto.BrandImage) };
                            context.Brands.Add(brand);
                        }
                        processedBrands.Add(dto.BrandName, brand);
                    }

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