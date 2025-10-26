using Ecommerce.DTOs;
using Ecommerce.Entity;

namespace Ecommerce.Interfaces.Services;

public interface IBrandService
{
    List<Brand> GetAll();
    Brand CreateBrand(CreateBrandDto brandDto);
    bool DeleteBrand(int id);
    //Method put da marca 
    Brand? UpdateBrand(int id, CreateBrandDto brandDto);
}