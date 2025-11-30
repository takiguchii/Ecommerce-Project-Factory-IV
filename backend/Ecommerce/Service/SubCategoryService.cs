using Ecommerce.DTOs;
using Ecommerce.Entity;
using Ecommerce.Interfaces.Repositories;
using Ecommerce.Interfaces.Services;

namespace Ecommerce.Service;

public class SubCategoryService : ISubCategoryService
{
    private readonly ISubCategoryRepository _subCategoryRepository;
    private readonly ICategoryRepository _categoryRepository; 

    public SubCategoryService(ISubCategoryRepository subCategoryRepository, ICategoryRepository categoryRepository)
    {
        _subCategoryRepository = subCategoryRepository;
        _categoryRepository = categoryRepository;
    }

    public object? CreateSubCategory(CreateSubCategoryDto subCategoryDto)
    {
        var parentCategory = _categoryRepository.GetById(subCategoryDto.category_id);
        if (parentCategory == null)
        {
            return new { Message = "A categoria pai informada não existe." };
        }

        var newSubCategory = new SubCategory
        {
            name = subCategoryDto.name,
            category_id = subCategoryDto.category_id
        };

        _subCategoryRepository.Add(newSubCategory);
        _subCategoryRepository.SaveChanges();
        return newSubCategory;
    }
    
    public List<SubCategory> GetAllSubCategories()
    {
        return _subCategoryRepository.GetAll();
    }

    public SubCategory? GetSubCategoryById(int id)
    {
        return _subCategoryRepository.GetById(id);
    }
    public bool DeleteSubCategory(int id)
    {
        var subCategory = _subCategoryRepository.GetById(id);

        if (subCategory == null)
        {
            return false;
        }

        _subCategoryRepository.Delete(subCategory);
        _subCategoryRepository.SaveChanges();
        return true;
    }
    public SubCategory? UpdateSubCategory(int id, CreateSubCategoryDto subCategoryDto)
    {
        var existingSubCategory = _subCategoryRepository.GetById(id);

        if (existingSubCategory == null)
        {
            return null;
        }

        existingSubCategory.name = subCategoryDto.name;
        existingSubCategory.category_id = subCategoryDto.category_id; 

        _subCategoryRepository.Update(existingSubCategory);
        _subCategoryRepository.SaveChanges();

        return existingSubCategory;
    }
    public List<SubCategory> GetSubCategoriesByCategoryId(int categoryId)
    {
        return _subCategoryRepository.GetAll()
            .Where(s => s.category_id == categoryId)
            .ToList();
    }
}