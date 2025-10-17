using Ecommerce.Dto;
using Ecommerce.Entity;
using Ecommerce.Interfaces;
using Ecommerce.Interfaces.Repositories; 
using Ecommerce.DTOs;

namespace Ecommerce.Service;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Product CreateProduct(CreateProductDto productDto)
    {
        var newProduct = new Product
        {
            Name = productDto.Name,
            CoverImageUrl = productDto.CoverImageUrl,
            OriginalPrice = productDto.OriginalPrice,
            DiscountPrice = productDto.DiscountPrice,
            Description = productDto.Description,
            TechnicalInfo = productDto.TechnicalInfo,
            RawDescription = productDto.RawDescription,
            RawTechnicalInfo = productDto.RawTechnicalInfo,

        };

        _productRepository.Add(newProduct);
        _productRepository.SaveChanges();

        return newProduct;
    }

    public List<Product> GetAllProducts()
    {
        return _productRepository.GetAll();
    }

    public Product? GetProductById(int id)
    {
        return _productRepository.GetById(id);
    }
    public bool DeleteProduct(int id)
    {
        var product = _productRepository.GetById(id);

        if (product == null)
        {
            return false;
        }
        _productRepository.Delete(product);
        _productRepository.SaveChanges();
        return true; 
    }
    public List<Product> GetPromotions()
    {
        return _productRepository.GetPromotions();
    }
    public async Task<CreatePaginatedResultDto<Product>> GetProductsPaginatedAsync(int pageNumber, int pageSize, int? categoryId, int? subCategoryId, int? brandId)
    {
        return await _productRepository.GetProductsPaginatedAsync(pageNumber, pageSize, categoryId, subCategoryId, brandId);
    }
  
        public async Task<List<ProductSearchSuggestionDto>> GetSearchSuggestionsAsync(string searchTerm)
        {
            const int suggestionLimit = 5; // Limitandoo a sugestão para 5 itens 
            
            if (string.IsNullOrWhiteSpace(searchTerm) || searchTerm.Length < 3) // Definindo o limite para aparecer a sugestão ( só aprece depois de 3 caracters )
            {
                return new List<ProductSearchSuggestionDto>(); 
            }
    
            return await _productRepository.GetSearchSuggestionsAsync(searchTerm, suggestionLimit);
        }
    }
