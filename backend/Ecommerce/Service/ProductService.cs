using Ecommerce.Dto;
using Ecommerce.Entity;
using Ecommerce.Interfaces.Repositories; 
using Ecommerce.DTOs;
using Ecommerce.Interfaces.Services;

namespace Ecommerce.Service;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private static readonly Random _random = new Random();
    
    private readonly ICategoryRepository _categoryRepository;
    private readonly ISubCategoryRepository _subCategoryRepository;
    private readonly IBrandRepository _brandRepository;
    private readonly IProviderRepository _providerRepository;
    
    public ProductService(IProductRepository productRepository, 
                          ICategoryRepository categoryRepository, 
                          ISubCategoryRepository subCategoryRepository, 
                          IBrandRepository brandRepository, 
                          IProviderRepository providerRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
        _subCategoryRepository = subCategoryRepository;
        _brandRepository = brandRepository;
        _providerRepository = providerRepository;
    }
    
    public Product? CreateProduct(CreateProductDto productDto)
    {
        var category = _categoryRepository.GetById(productDto.category_id);
        if (category == null) return null;

        var brand = _brandRepository.GetById(productDto.brand_id);
        if (brand == null) return null; 

        var provider = _providerRepository.GetById(productDto.provider_id);
        if (provider == null) return null; 

        if (productDto.sub_category_id != null && productDto.sub_category_id.Value != 0)
        {
            var subCategory = _subCategoryRepository.GetById(productDto.sub_category_id.Value);
            if (subCategory == null) return null; 
        }

        var newProduct = new Product
        {
            name = productDto.name,
            code = productDto.code, 
            image_url0 = productDto.image_url0,
            image_url1 = productDto.image_url1, 
            image_url2 = productDto.image_url2, 
            image_url3 = productDto.image_url3, 
            image_url4 = productDto.image_url4, 
            original_price = productDto.original_price,
            discount_price = productDto.discount_price,
            description = productDto.description,
            technical_info = productDto.technical_info,
            // minhas fks de produto 
            category_id = productDto.category_id,
            sub_category_id = productDto.sub_category_id,
            brand_id = productDto.brand_id,
            provider_id = productDto.provider_id,
        };

        _productRepository.Add(newProduct);
        _productRepository.SaveChanges(); 

        return newProduct;
    }
    
    public List<Product> GetAllProducts()
    {
        return _productRepository.GetAll();
    }

    public Product? GetProductById(int id)
    {
        return _productRepository.GetById(id);
    }
    
    public bool DeleteProduct(int id)
    {
        var product = _productRepository.GetById(id);

        if (product == null)
        {
            return false;
        }
        _productRepository.Delete(product);
        _productRepository.SaveChanges();
        return true; 
    }
    
    public Product? UpdateProduct(int id, CreateProductDto productDto)
    {
        var existingProduct = _productRepository.GetById(id);

        if (existingProduct == null)
        {
            return null;
        }

        var category = _categoryRepository.GetById(productDto.category_id);
        if (category == null) return null; 

        var brand = _brandRepository.GetById(productDto.brand_id);
        if (brand == null) return null; 

        var provider = _providerRepository.GetById(productDto.provider_id);
        if (provider == null) return null; 

        if (productDto.sub_category_id != null && productDto.sub_category_id.Value != 0)
        {
            var subCategory = _subCategoryRepository.GetById(productDto.sub_category_id.Value);
            if (subCategory == null) return null; 
        }

        existingProduct.code = productDto.code;
        existingProduct.name = productDto.name;
        existingProduct.original_price = productDto.original_price;
        existingProduct.discount_price = productDto.discount_price;
        existingProduct.description = productDto.description;
        existingProduct.technical_info = productDto.technical_info;
        
        existingProduct.image_url0 = productDto.image_url0;
        existingProduct.image_url1 = productDto.image_url1;
        existingProduct.image_url2 = productDto.image_url2;
        existingProduct.image_url3 = productDto.image_url3;
        existingProduct.image_url4 = productDto.image_url4;
        
        existingProduct.category_id = productDto.category_id;
        existingProduct.sub_category_id = productDto.sub_category_id;
        existingProduct.brand_id = productDto.brand_id;
        existingProduct.provider_id = productDto.provider_id;

        _productRepository.Update(existingProduct);
        _productRepository.SaveChanges();

        return existingProduct;
    }
    
    public List<Product> GetPromotions()
    {
        var potentialPromotions = _productRepository.GetPromotions();
        
        var actualPromotions = potentialPromotions
            .Where(p => p.original_price != null && p.discount_price != null)
            .ToList();

        var shuffledPromotions = actualPromotions.OrderBy(p => _random.Next());
        
        var selectedPromotions = shuffledPromotions.Take(12).ToList();

        return selectedPromotions;
    }
    
    public async Task<CreatePaginatedResultDto<Product>> GetProductsPaginatedAsync(int pageNumber, int pageSize, int? categoryId, int? subCategoryId, int? brandId)
    {
        // Busca os dados brutos do repositório
        var (items, totalCount) = await _productRepository.GetProductsPaginatedAsync(pageNumber, pageSize, categoryId, subCategoryId, brandId);

        // Monta o DTO 
        return new CreatePaginatedResultDto<Product>
        {
            Items = items,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }
  
    public async Task<List<ProductSearchSuggestionDto>> GetSearchSuggestionsAsync(string searchTerm)
    {
        const int suggestionLimit = 5; 
            
        if (string.IsNullOrWhiteSpace(searchTerm) || searchTerm.Length < 3) 
        { 
            return new List<ProductSearchSuggestionDto>(); 
        }
    
        return await _productRepository.GetSearchSuggestionsAsync(searchTerm, suggestionLimit);
    }
    
    public async Task<List<Product>> GetRandomProductsAsync(int? categoryId, int? subCategoryId, int? brandId)
    {
        const int productLimit = 12;

        var filteredProducts = await _productRepository.GetFilteredProductsAsync(categoryId, subCategoryId, brandId);
        
        var shuffledProducts = filteredProducts.OrderBy(p => _random.Next());
        
        // pega os primeiros 12.
        var selectedProducts = shuffledProducts.Take(productLimit).ToList();

        return selectedProducts;
    }
    public List<Product> GetProductsByCategory(int categoryId, int? subCategoryId = null)
    {

        var query = _productRepository.GetAll()
            .Where(p => p.category_id == categoryId); 
        
        if (subCategoryId.HasValue)
        {
            query = query.Where(p => p.sub_category_id == subCategoryId.Value);
        }
        
        return query.ToList();
    }
    public List<Product> GetProductsByCategory(int categoryId, int? subCategoryId = null, string? sort = null)
    {
        var query = _productRepository.GetAll()
            .Where(p => p.category_id == categoryId); 

        if (subCategoryId.HasValue)
        {
            query = query.Where(p => p.sub_category_id == subCategoryId.Value);
        }

        if (!string.IsNullOrEmpty(sort))
        {
            if (sort == "price_asc") 
            {
                query = query.OrderBy(p => 
                    decimal.TryParse(p.discount_price, out var price) ? price : 0);
            }
            else if (sort == "price_desc") 
            {
                query = query.OrderByDescending(p => 
                    decimal.TryParse(p.discount_price, out var price) ? price : 0);
            }
        }
    
        return query.ToList();
    }
}