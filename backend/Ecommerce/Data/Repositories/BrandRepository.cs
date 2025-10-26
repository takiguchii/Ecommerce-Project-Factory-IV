using Ecommerce.Data.Context;
using Ecommerce.Entity;
using Ecommerce.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Repositories;

public class BrandRepository : IBrandRepository
{
    private readonly EcommerceDbContext _context;
    public BrandRepository(EcommerceDbContext context)
    {
        _context = context;
    }
    public List<Brand> GetAll()
    {
        return _context.Brands.ToList();
    }
    public Brand? GetById(int id)
    {
        return _context.Brands.FirstOrDefault(b => b.id == id);
    }
    public void Add(Brand brand)
    {
        _context.Brands.Add(brand);
    }
    public void Delete(Brand brand)
    {
        _context.Brands.Remove(brand);
    }
    public void SaveChanges()
    {
        _context.SaveChanges();
    }
    public void Update(Brand brand)
    {
        _context.Entry(brand).State = EntityState.Modified; 
    }
}