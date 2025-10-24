using Ecommerce.DTOs;
using Ecommerce.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization; 

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

    // ABERTO: Todos podem ver as marcas
    [HttpGet]
    public IActionResult GetAllBrands()
    {
        var brands = _brandService.GetAll();
        return Ok(brands);
    }

    // TRANCADO: Só pode Acesar quem for Adm 
    [HttpPost]
    [Authorize(Roles = "Admin")] // <-- 2. A TRAVA
    public IActionResult CreateBrand([FromBody] CreateBrandDto brandDto)
    {
        var brand = _brandService.CreateBrand(brandDto);
        return CreatedAtAction(nameof(GetAllBrands), new { id = brand.Id }, brand);
    }

    // TRANCADO: Só pode acessar quem for adm 
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")] 
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