using Microsoft.AspNetCore.Mvc;
using Ecommerce.DTOs;
using Ecommerce.Interfaces.Services;

namespace Ecommerce.Controller;

[ApiController]
[Route("api/categories")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpPost]
    public IActionResult CreateCategory([FromBody] CreateCategoryDto categoryDto)
    {
        var category = _categoryService.CreateCategory(categoryDto);
        return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
    }

    [HttpGet]
    public IActionResult GetAllCategories()
    {
        var categories = _categoryService.GetAllCategories();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public IActionResult GetCategoryById(int id)
    {
        var category = _categoryService.GetCategoryById(id);
        if (category == null)
        {
            return NotFound();
        }
        return Ok(category);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteCategory(int id)
    {
        var success = _categoryService.DeleteCategory(id);
        if (!success)
        {
            return NotFound();
        }
        return NoContent();
    }
}