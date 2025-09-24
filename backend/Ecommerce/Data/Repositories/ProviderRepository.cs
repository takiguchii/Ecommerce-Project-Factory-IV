using Ecommerce.Data.Context;
using Ecommerce.Entity;
using Ecommerce.Interfaces.Repositories;

namespace Ecommerce.Repositories;

public class ProviderRepository : IProviderRepository
{
    private readonly EcommerceDbContext _dbContext;

    public ProviderRepository(EcommerceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(Provider provider)
    {
        _dbContext.Providers.Add(provider);
    }
    
    public List<Provider> GetAll()
    {
        return _dbContext.Providers.ToList();
    }

    public Provider? GetById(int id)
    {
        return _dbContext.Providers.FirstOrDefault(p => p.Id == id);
    }

    public Provider? GetByCnpj(string cnpj)
    {
        return _dbContext.Providers.FirstOrDefault(p => p.Cnpj == cnpj);
    }

    public void SaveChanges()
    {
        _dbContext.SaveChanges();
    }
}