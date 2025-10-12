using Ecommerce.Dto;
using Ecommerce.Entity;
using Ecommerce.Interfaces;
using Ecommerce.Interfaces.Repositories; 
using Ecommerce.Interfaces.Services;
using Ecommerce.DTOs;

namespace Ecommerce.Service;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public Product CreateProduct(CreateProductDto productDto)
    {
        // Futuras regras de negocio, Por enquanto apenas os DTOS

        var newProduct = new Product
        {
            Name = productDto.Name,
            CoverImageUrl = productDto.CoverImageUrl,
            OriginalPrice = productDto.OriginalPrice,
            DiscountPrice = productDto.DiscountPrice,
            Description = productDto.Description,
            TechnicalInfo = productDto.TechnicalInfo,
            RawDescription = productDto.RawDescription,
            RawTechnicalInfo = productDto.RawTechnicalInfo,

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
    public List<Product> GetPromotions()
    {
        return _productRepository.GetPromotions();
    }
    public async Task<CreatePaginatedResultDto<Product>> GetByCategoryPaginatedAsync(int categoryId, int pageNumber, int pageSize)
    {
        return await _productRepository.GetByCategoryPaginatedAsync(categoryId, pageNumber, pageSize);
    }
}