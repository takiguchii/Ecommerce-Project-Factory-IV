using Microsoft.AspNetCore.Mvc;
using Ecommerce.Dto;
using Ecommerce.Entity;
using Ecommerce.Interfaces; 

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
            return NotFound(); // Retorna 404 se não encontrar o produto
        }

        return Ok(product);
    }

    [HttpGet]
    public IActionResult GetAllProducts()
    {
        var products = _productService.GetAllProducts();
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