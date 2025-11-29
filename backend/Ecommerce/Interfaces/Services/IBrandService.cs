using Ecommerce.DTOs;
using Ecommerce.Entity;

namespace Ecommerce.Interfaces.Services;

public interface IBrandService
{
    List<Brand> GetAll();
    List<Brand> GetRandomBrands(int count);
    Brand CreateBrand(CreateBrandDto brandDto);
    bool DeleteBrand(int id);
    Brand? UpdateBrand(int id, CreateBrandDto brandDto);
}