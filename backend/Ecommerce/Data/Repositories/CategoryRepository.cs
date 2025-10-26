using Ecommerce.Data.Context;
using Ecommerce.Entity;
using Ecommerce.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Ecommerce.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly EcommerceDbContext _dbContext;

    public CategoryRepository(EcommerceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(Category category)
    {
        _dbContext.Categories.Add(category);
    }
    
    public List<Category> GetAll()
    {
        return _dbContext.Categories.ToList();
    }

    public Category? GetById(int id)
    {
        return _dbContext.Categories.FirstOrDefault(c => c.id == id);
    }
    
    public void Delete(Category category)
    {
        _dbContext.Categories.Remove(category);
    }
    public void Update(Category category)
    {
        _dbContext.Entry(category).State = EntityState.Modified;
    }

    public void SaveChanges()
    {
        _dbContext.SaveChanges();
    }
    
}