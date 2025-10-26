using Ecommerce.DTOs;
using Ecommerce.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Ecommerce.Api.Controllers 
{
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
        [Authorize(Roles = "Admin")]
        public IActionResult CreateBrand([FromBody] CreateBrandDto brandDto)
        {
            var brand = _brandService.CreateBrand(brandDto);

            return CreatedAtAction(nameof(GetAllBrands), new { id = brand.id }, brand); 
        }
        
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateBrand(int id, [FromBody] CreateBrandDto brandDto)
        {
            var updatedBrand = _brandService.UpdateBrand(id, brandDto);

            if (updatedBrand == null)
            {
                return NotFound($"Marca com ID {id} não encontrada.");
            }
            return Ok(updatedBrand);
        }

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
}