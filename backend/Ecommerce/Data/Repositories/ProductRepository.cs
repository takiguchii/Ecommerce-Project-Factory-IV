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
        return _dbContext.Products.FirstOrDefault(p => p.id == id);
    }
    public void SaveChanges()
    {
        _dbContext.SaveChanges();
    }
    public List<Product> GetPromotions()
    {
        return _dbContext.Products
            .Where(p => p.discount_price != null)
            .ToList();
    }
    public async Task<List<ProductSearchSuggestionDto>> GetSearchSuggestionsAsync(string searchTerm, int limit)
    {
        return await _dbContext.Products
            .Where(p => p.name.ToLower().Contains(searchTerm.ToLower()))
            .Select(p => new ProductSearchSuggestionDto
            {
                Id = p.id,
                Name = p.name,
                CoverImageUrl = p.image_url0
            })
            .Take(limit)
            .ToListAsync();
    }
    
    //Paginação ( em teste usando o Dto ) 
    public async Task<CreatePaginatedResultDto<Product>> GetProductsPaginatedAsync(int pageNumber, int pageSize, int? categoryId, int? subCategoryId, int? brandId)
    {
        var query = _dbContext.Products.AsQueryable();

        // Filtro dinamico ( Em teste )
        if (categoryId.HasValue)
        {
            query = query.Where(p => p.category_id == categoryId.Value);
        }
        if (subCategoryId.HasValue)
        {
            query = query.Where(p => p.sub_category_id == subCategoryId.Value);
        }
        if (brandId.HasValue)
        {
            query = query.Where(p => p.brand_id == brandId.Value);
        }

        var totalCount = await query.CountAsync();

        // Aplica a paginação
        var items = await query.Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // Retorna o resultado no DTO
        return new CreatePaginatedResultDto<Product>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
    public async Task<List<Product>> GetFilteredProductsAsync(int? categoryId, int? subCategoryId, int? brandId)
    {
        var query = _dbContext.Products.AsQueryable();

        if (categoryId.HasValue)
        {
            query = query.Where(p => p.category_id == categoryId.Value);
        }
        if (subCategoryId.HasValue)
        {
            query = query.Where(p => p.sub_category_id == subCategoryId.Value);
        }
        if (brandId.HasValue)
        {
            query = query.Where(p => p.brand_id == brandId.Value);
        }

        return await query.ToListAsync();
    }

}