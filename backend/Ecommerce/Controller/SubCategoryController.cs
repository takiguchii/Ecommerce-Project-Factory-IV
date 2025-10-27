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
            return CreatedAtAction(nameof(GetSubCategoryById), new { id = subCategory.id }, subCategory);
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
    [HttpPut("{id}")]
    public IActionResult UpdateSubCategory(int id, [FromBody] CreateSubCategoryDto subCategoryDto)
    {
        var updatedSubCategory = _subCategoryService.UpdateSubCategory(id, subCategoryDto);

        if (updatedSubCategory == null)
        {
            return NotFound($"Subcategoria com ID {id} não encontrada.");
        }

        return Ok(updatedSubCategory);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteSubCategory(int id)
    {
        var success = _subCategoryService.DeleteSubCategory(id);
        if (!success)
        {
            return NotFound($"Subcategoria com ID {id} não encontrada.");
        }
        return NoContent();
    }
}