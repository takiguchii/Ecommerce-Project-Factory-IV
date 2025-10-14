using Ecommerce.Dto;
using Ecommerce.Entity;
using Ecommerce.DTOs;


namespace Ecommerce.Interfaces;

public interface IProductService
{
    Product CreateProduct(CreateProductDto productDto);
    List<Product> GetAllProducts();
    Product? GetProductById(int id);
    bool DeleteProduct(int id);
    List<Product> GetPromotions();
    //Task<CreatePaginatedResultDto<Product>> GetByCategoryPaginatedAsync(int categoryId, int pageNumber, int pageSize);
    Task<CreatePaginatedResultDto<Product>> GetProductsPaginatedAsync(int pageNumber, int pageSize, int? categoryId, int? subCategoryId, int? brandId);


}