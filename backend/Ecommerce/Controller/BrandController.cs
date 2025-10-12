using Ecommerce.DTOs;
using Ecommerce.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers;

[ApiController]
[Route("api/brands")]
public class BrandController : ControllerBase
{
    private readonly IBrandService _brandService;

    public BrandController(IBrandService brandService)
    {
        _brandService = brandService;
    }

    [HttpGet]
    public IActionResult GetAllBrands()
    {
        var brands = _brandService.GetAll();
        return Ok(brands);
    }

    [HttpPost]
    public IActionResult CreateBrand([FromBody] CreateBrandDto brandDto)
    {
        var brand = _brandService.CreateBrand(brandDto);
        return CreatedAtAction(nameof(GetAllBrands), new { id = brand.Id }, brand);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteBrand(int id)
    {
        var success = _brandService.DeleteBrand(id);
        if (!success)
        {
            return NotFound();
        }
        return NoContent();
    }
}