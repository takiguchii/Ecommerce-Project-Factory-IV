using Ecommerce.Entity;

namespace Ecommerce.Interfaces.Repositories;

public interface IBrandRepository
{
    Task<List<Brand>> GetAllAsync();
}