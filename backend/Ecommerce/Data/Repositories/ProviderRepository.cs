using Ecommerce.Data.Context;
using Ecommerce.Entity;
using Ecommerce.Interfaces.Repositories;
using Ecommerce.DTOs;
using Microsoft.EntityFrameworkCore; 

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
        return _dbContext.Providers.FirstOrDefault(p => p.id == id);
    }

    public Provider? GetByCnpj(string cnpj)
    {
        return _dbContext.Providers.FirstOrDefault(p => p.cnpj == cnpj);
    }
    public void Delete(Provider provider)
    {
        _dbContext.Providers.Remove(provider);
    }
    public void SaveChanges()
    {
        _dbContext.SaveChanges();
    }
    public void Update(Provider provider)
    {
        _dbContext.Entry(provider).State = EntityState.Modified;
    }
}