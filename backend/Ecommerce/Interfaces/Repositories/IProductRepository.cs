using Ecommerce.Entity;
using Ecommerce.DTOs; 

namespace Ecommerce.Interfaces.Repositories;

public interface IProductRepository
{
    void Add(Product product);
    List<Product> GetAll();
    Product? GetById(int id);
    void Delete(Product product);
    void SaveChanges();
    List<Product> GetPromotions(); 
    void Update(Product product);
    
    Task<(List<Product> Items, int TotalCount)> GetProductsPaginatedAsync(int pageNumber, int pageSize, int? categoryId, int? subCategoryId, int? brandId);
    Task<List<ProductSearchSuggestionDto>> GetSearchSuggestionsAsync(string searchTerm, int limit);
    Task<List<Product>> GetFilteredProductsAsync(int? categoryId, int? subCategoryId, int? brandId);
}