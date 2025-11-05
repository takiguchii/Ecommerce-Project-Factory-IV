using Ecommerce.Dto;
using Ecommerce.Entity;
using Ecommerce.DTOs;

namespace Ecommerce.Interfaces.Services 
{
    public interface IProductService
    {
        Product? CreateProduct(CreateProductDto productDto);
        List<Product> GetAllProducts();
        Product? GetProductById(int id);
        bool DeleteProduct(int id);
        Product? UpdateProduct(int id, CreateProductDto productDto);
        List<Product> GetPromotions();
        Task<CreatePaginatedResultDto<Product>> GetProductsPaginatedAsync(int pageNumber, int pageSize, int? categoryId, int? subCategoryId, int? brandId);
        Task<List<ProductSearchSuggestionDto>> GetSearchSuggestionsAsync(string searchTerm);
        Task<List<Product>> GetRandomProductsAsync(int? categoryId, int? subCategoryId, int? brandId);
    }
}