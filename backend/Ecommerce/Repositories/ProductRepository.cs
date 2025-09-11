using Ecommerce.Data;
using Ecommerce.Entity;
using Ecommerce.Interfaces; 

namespace Ecommerce.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly EcommerceDbContext _dbContext;

    public ProductRepository(EcommerceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(Product product)
    {
        _dbContext.Products.Add(product);
    }

    public List<Product> GetAll()
    {
        return _dbContext.Products.ToList();
    }

    public Product? GetById(int id)
    {
        return _dbContext.Products.FirstOrDefault(p => p.Id == id);
    }

    public void SaveChanges()
    {
        _dbContext.SaveChanges();
    }
}