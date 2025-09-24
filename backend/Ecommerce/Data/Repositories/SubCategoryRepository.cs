using Ecommerce.Data.Context;
using Ecommerce.Entity;
using Ecommerce.Interfaces.Repositories;

namespace Ecommerce.Repositories;

public class SubCategoryRepository : ISubCategoryRepository
{
    private readonly EcommerceDbContext _dbContext;

    public SubCategoryRepository(EcommerceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(SubCategory subCategory)
    {
        _dbContext.SubCategories.Add(subCategory);
    }
    
    public List<SubCategory> GetAll()
    {
        return _dbContext.SubCategories.ToList();
    }

    public SubCategory? GetById(int id)
    {
        return _dbContext.SubCategories.FirstOrDefault(sc => sc.Id == id);
    }

    public void SaveChanges()
    {
        _dbContext.SaveChanges();
    }
}