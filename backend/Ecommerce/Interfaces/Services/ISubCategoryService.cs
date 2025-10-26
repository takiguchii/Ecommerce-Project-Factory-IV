using Ecommerce.DTOs;
using Ecommerce.Entity;

namespace Ecommerce.Interfaces.Services;

public interface ISubCategoryService
{
    object? CreateSubCategory(CreateSubCategoryDto subCategoryDto); 
    List<SubCategory> GetAllSubCategories();
    SubCategory? GetSubCategoryById(int id);
    bool DeleteSubCategory(int id);
    SubCategory? UpdateSubCategory(int id, CreateSubCategoryDto subCategoryDto);
}