using Ecommerce.Entity;

namespace Ecommerce.Interfaces.Repositories;
public interface IProductRepository
{
    void Add(Product product);
    List<Product> GetAll();
    Product? GetById(int id);
    void Delete(Product product);
    void SaveChanges();
    List<Product> GetPromotions(); 

}