using Microsoft.AspNetCore.Mvc;
using Ecommerce.Dto;
using Ecommerce.Entity;
using Ecommerce.Interfaces;
using Ecommerce.DTOs; 

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
        
        return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
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
    
    [HttpGet("category/{categoryId}")]
    public async Task<ActionResult<CreatePaginatedResultDto<Product>>> GetProductsByCategory(int categoryId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var products = await _productService.GetByCategoryPaginatedAsync(categoryId, pageNumber, pageSize);
        
        if (products == null || !products.Items.Any())
        {
            return NotFound("Nenhum produto encontrado para esta categoria.");
        }
        
        return Ok(products);
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