using Ecommerce.Dto;
using Ecommerce.DTOs;
using Ecommerce.Entity;

namespace Ecommerce.Interfaces.Services;

public interface IProductService
{
    Product? CreateProduct(CreateProductDto productDto);
    
    List<Product> GetAllProducts();
    
    Product? GetProductById(int id);
    
    bool DeleteProduct(int id);
    
    Product? UpdateProduct(int id, CreateProductDto productDto);
    
    List<Product> GetPromotions();
    
    Task<CreatePaginatedResultDto<Product>> GetProductsPaginatedAsync(
        int pageNumber, 
        int pageSize, 
        int? categoryId, 
        int? subCategoryId, 
        int? brandId, 
        string? sortOrder = null);
    
    Task<List<ProductSearchSuggestionDto>> GetSearchSuggestionsAsync(string searchTerm);
    
    Task<List<Product>> GetRandomProductsAsync(int? categoryId, int? subCategoryId, int? brandId);

    List<Product> GetProductsByCategory(int categoryId, int? subCategoryId = null, string? sort = null);
}