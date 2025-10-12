using Ecommerce.DTOs;
using Ecommerce.Entity;
using Ecommerce.Interfaces.Repositories;
using Ecommerce.Interfaces.Services;

namespace Ecommerce.Service;

public class BrandService : IBrandService
{
    private readonly IBrandRepository _brandRepository;

    public BrandService(IBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }

    public List<Brand> GetAll()
    {
        return _brandRepository.GetAll();
    }

    public Brand CreateBrand(CreateBrandDto brandDto)
    {
        var newBrand = new Brand
        {
            Name = brandDto.Name,
            ImageUrl = brandDto.ImageUrl
        };
        _brandRepository.Add(newBrand);
        _brandRepository.SaveChanges();
        return newBrand;
    }

    public bool DeleteBrand(int id)
    {
        var brand = _brandRepository.GetById(id);
        if (brand == null)
        {
            return false;
        }
        _brandRepository.Delete(brand);
        _brandRepository.SaveChanges();
        return true;
    }
}