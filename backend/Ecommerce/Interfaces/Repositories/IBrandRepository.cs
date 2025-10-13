using Ecommerce.Entity;

namespace Ecommerce.Interfaces.Repositories;

public interface IBrandRepository
{
    List<Brand> GetAll(); 
    Brand? GetById(int id);
    void Add(Brand brand);
    void Delete(Brand brand);
    void SaveChanges();
}