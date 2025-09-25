using Ecommerce.DTOs;
using Ecommerce.Entity;
using Ecommerce.Interfaces.Repositories;
using Ecommerce.Interfaces.Services;

namespace Ecommerce.Service;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public Category CreateCategory(CreateCategoryDto categoryDto)
    {
        var newCategory = new Category { Name = categoryDto.Name };
        _categoryRepository.Add(newCategory);
        _categoryRepository.SaveChanges();
        return newCategory;
    }
    
    public List<Category> GetAllCategories()
    {
        return _categoryRepository.GetAll();
    }

    public Category? GetCategoryById(int id)
    {
        return _categoryRepository.GetById(id);
    }

    public bool DeleteCategory(int id)
    {
        var category = _categoryRepository.GetById(id);
        if (category == null)
        {
            return false;
        }
        _categoryRepository.Delete(category);
        _categoryRepository.SaveChanges();
        return true;
    }
}