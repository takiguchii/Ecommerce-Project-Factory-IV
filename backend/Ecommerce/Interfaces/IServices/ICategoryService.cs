using Ecommerce.DTOs;
using Ecommerce.Entity;

namespace Ecommerce.Interfaces.Services;

public interface ICategoryService
{
    Category CreateCategory(CreateCategoryDto categoryDto);
    List<Category> GetAllCategories();
    Category? GetCategoryById(int id);
    bool DeleteCategory(int id);
}