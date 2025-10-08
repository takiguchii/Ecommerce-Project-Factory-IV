using Ecommerce.Data.Context;
using Ecommerce.Entity;
using Ecommerce.Interfaces.Repositories;
using Ecommerce.DTOs;
using Microsoft.EntityFrameworkCore; 


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
    public void Delete(Product product)
    {
        _dbContext.Products.Remove(product);
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
    public List<Product> GetPromotions()
    {
        return _dbContext.Products
            .Where(p => p.DiscountPrice != null)
            .ToList();
    }
    
    //METODO DA PAGINAÇÃO
    public async Task<PaginatedResult<Product>> GetByCategoryPaginatedAsync(int categoryId, int pageNumber, int pageSize)
    {
        //  ANOTAÇÕES PARA O FRONT
        
        // 1. Cria a consulta base, filtrando por categoria
        var query = _dbContext.Products
            .Where(p => p.CategoryId == categoryId)
            .AsQueryable();

        // 2. Conta o total de itens (de forma assíncrona)
        var totalCount = await query.CountAsync();

        // 3. Executa a paginação na consulta
        var items = await query.Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // 4. Retorna o resultado no nosso DTO de paginação
        return new PaginatedResult<Product>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
    
}