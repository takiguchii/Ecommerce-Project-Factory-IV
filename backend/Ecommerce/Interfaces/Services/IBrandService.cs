using Ecommerce.Entity;

namespace Ecommerce.Interfaces.Services;

public interface IBrandService
{
    Task<List<Brand>> GetAllAsync();
}