using Microsoft.AspNetCore.Mvc;
using Ecommerce.Dto;
using Ecommerce.Entity;
using Ecommerce.Interfaces; 
using Ecommerce.DTOs; 
using System.Linq; 
using System.Threading.Tasks;
using Ecommerce.Interfaces.Services; 

namespace Ecommerce.Api.Controllers; 

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    
    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }
    
    [HttpPost]
    public IActionResult CreateProduct([FromBody] CreateProductDto productDto)
    {
        var product = _productService.CreateProduct(productDto);
        
        if (product == null)
        {
            return BadRequest(new { message = "Não foi possível criar o produto. Um dos IDs (Categoria, SubCategoria, Marca ou Fornecedor) é inválido." });
        }

        return CreatedAtAction(nameof(GetProductById), new { id = product.id }, product);
    }

    [HttpGet("{id}")]
    public IActionResult GetProductById(int id)
    {
        var product = _productService.GetProductById(id);

        if (product == null)
        {
            return NotFound(); 
        }

        return Ok(product);
    }

    [HttpGet]
    public IActionResult GetAllProducts()
    {
        var products = _productService.GetAllProducts();
        return Ok(products);
    }
    
    [HttpGet("promotions")] 
    public IActionResult GetPromotions()
    {
        var promotionalProducts = _productService.GetPromotions();
        return Ok(promotionalProducts);
    }
    
    [HttpGet("filter")] 
    public async Task<ActionResult<CreatePaginatedResultDto<Product>>> GetProductsByFilter(
        [FromQuery] int pageNumber = 1, 
        [FromQuery] int pageSize = 10,
        [FromQuery] int? categoryId = null,
        [FromQuery] int? subCategoryId = null,
        [FromQuery] int? brandId = null)
    {
        var result = await _productService.GetProductsPaginatedAsync(pageNumber, pageSize, categoryId, subCategoryId, brandId);
        
        if (result.TotalCount == 0)
        {
            return NotFound("Nenhum produto encontrado para os filtros aplicados.");
        }
        
        return Ok(result);
    }
    
    [HttpGet("search-suggestions")]
    public async Task<IActionResult> GetSearchSuggestions([FromQuery] string query)
    {
        var suggestions = await _productService.GetSearchSuggestionsAsync(query);
        return Ok(suggestions);
    }
    
    [HttpGet("productsGridHomePage")]
    public async Task<ActionResult<List<Product>>> GetRandomProducts(
        [FromQuery] int? categoryId = null,
        [FromQuery] int? subCategoryId = null,
        [FromQuery] int? brandId = null)
    {
        var products = await _productService.GetRandomProductsAsync(categoryId, subCategoryId, brandId);
        
        if (products == null || !products.Any())
        {
            return NotFound("Nenhum produto encontrado para os filtros aplicados.");
        }
        
        return Ok(products);
    }
    [HttpGet("category/{categoryId}")]
    public IActionResult GetByCategory(int categoryId, [FromQuery] int? subCategoryId = null)
    {
        var products = _productService.GetProductsByCategory(categoryId, subCategoryId);
        return Ok(products);
    }
    

    
    [HttpPut("{id}")]
    public IActionResult UpdateProduct(int id, [FromBody] CreateProductDto productDto)
    {
        var updatedProduct = _productService.UpdateProduct(id, productDto);

        if (updatedProduct == null)
        {
            return NotFound($"Produto com ID {id} não encontrado ou um dos IDs de Categoria/Marca é inválido.");
        }

        return Ok(updatedProduct);
    }
    
    [HttpDelete("{id}")]
    public IActionResult DeleteProduct(int id)
    {
        var success = _productService.DeleteProduct(id);
        if (!success)
        {
            return NotFound(); 
        }
        return NoContent(); 
    }
}
