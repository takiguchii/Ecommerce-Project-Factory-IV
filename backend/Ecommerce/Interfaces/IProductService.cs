using Ecommerce.Dto;
using Ecommerce.Entity;

namespace Ecommerce.Interfaces;

public interface IProductService
{
    Product CreateProduct(CreateProductDto productDto);
    List<Product> GetAllProducts();
    Product? GetProductById(int id);
    bool DeleteProduct(int id);
}