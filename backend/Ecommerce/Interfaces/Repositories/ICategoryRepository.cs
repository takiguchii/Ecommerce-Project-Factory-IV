using Ecommerce.Entity;

namespace Ecommerce.Interfaces.Repositories; 

public interface ICategoryRepository
{
    void Add(Category category);
    List<Category> GetAll();
    Category? GetById(int id);
    void Delete(Category category);
    void SaveChanges();
    //method put 
    void Update(Category category);
}