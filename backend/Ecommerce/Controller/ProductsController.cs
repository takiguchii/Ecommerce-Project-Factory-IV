using Microsoft.AspNetCore.Mvc;
using Ecommerce.Dto;
using Ecommerce.Entity; 

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    [HttpPost]
    public IActionResult CreateProduct([FromBody] CreateProductDto productDto)
    {
        var productEntity = new Product
        {
            Id = 1,
            Name = productDto.Name,
            OriginalPrice = productDto.OriginalPrice,
            Description = productDto.Description
        };
        
        return Ok(productEntity);
    }
    [HttpGet("{id}")]
    public IActionResult GetProductById(int id)
    {
        var productEntity = new Product
        {
            Id = id,
            Name = "Produto de Exemplo",
            OriginalPrice = 99.99m,
            Description = "Esta é uma descrição de exemplo."
        };
        return Ok(productEntity);
    }
}