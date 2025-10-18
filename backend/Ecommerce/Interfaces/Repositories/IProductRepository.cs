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
    //Task<CreatePaginatedResultDto<Product>> GetByCategoryPaginatedAsync(int categoryId, int pageNumber, int pageSize);
    Task<CreatePaginatedResultDto<Product>> GetProductsPaginatedAsync(int pageNumber, int pageSize, int? categoryId, int? subCategoryId, int? brandId);

    // Adicionando metodo da barra de pesquisa ( experimental ) 
    Task<List<ProductSearchSuggestionDto>> GetSearchSuggestionsAsync(string searchTerm, int limit);
}