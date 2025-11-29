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
            name = brandDto.name,
            brand_image_url = brandDto.brand_image_url
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
    // put da marca ( em teste ) 
    public Brand? UpdateBrand(int id, CreateBrandDto brandDto)
    {
        var existingBrand = _brandRepository.GetById(id);

        if (existingBrand == null)
        {
            return null;
        }

        existingBrand.name = brandDto.name;
        existingBrand.brand_image_url = brandDto.brand_image_url;

        _brandRepository.Update(existingBrand);
        _brandRepository.SaveChanges();

        return existingBrand;
    }
    public List<Brand> GetRandomBrands(int limit)
    {
        var allBrands = _brandRepository.GetAll();

        if (allBrands == null || !allBrands.Any())
        {
            return new List<Brand>();
        }

        var randomBrands = allBrands
            .OrderBy(x => Guid.NewGuid()) 
            .Take(limit) 
            .ToList();

        return randomBrands;
    }
}