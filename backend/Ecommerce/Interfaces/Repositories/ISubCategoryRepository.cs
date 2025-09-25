using Ecommerce.Entity;

namespace Ecommerce.Interfaces.Repositories;

public interface ISubCategoryRepository
{
    void Add(SubCategory subCategory);
    List<SubCategory> GetAll();
    SubCategory? GetById(int id);
    void SaveChanges();
}