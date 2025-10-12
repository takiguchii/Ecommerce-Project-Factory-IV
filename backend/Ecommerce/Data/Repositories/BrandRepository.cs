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

    public async Task<List<Brand>> GetAllAsync()
    {
        return await _context.Brands.ToListAsync();
    }
}