using Microsoft.AspNetCore.Mvc;
using Ecommerce.DTOs;
using Ecommerce.Entity;
using Ecommerce.Interfaces.Services;

namespace Ecommerce.Controller;

[ApiController]
[Route("api/subcategories")]
public class SubCategoryController : ControllerBase
{
    private readonly ISubCategoryService _subCategoryService;

    public SubCategoryController(ISubCategoryService subCategoryService)
    {
        _subCategoryService = subCategoryService;
    }

    [HttpPost]
    public IActionResult CreateSubCategory([FromBody] CreateSubCategoryDto subCategoryDto)
    {
        var result = _subCategoryService.CreateSubCategory(subCategoryDto);

        if (result is SubCategory subCategory)
        {
            return CreatedAtAction(nameof(GetSubCategoryById), new { id = subCategory.Id }, subCategory);
        }
        
        return BadRequest(result);
    }

    [HttpGet]
    public IActionResult GetAllSubCategories()
    {
        var subCategories = _subCategoryService.GetAllSubCategories();
        return Ok(subCategories);
    }

    [HttpGet("{id}")]
    public IActionResult GetSubCategoryById(int id)
    {
        var subCategory = _subCategoryService.GetSubCategoryById(id);
        if (subCategory == null)
        {
            return NotFound();
        }
        return Ok(subCategory);
    }
}