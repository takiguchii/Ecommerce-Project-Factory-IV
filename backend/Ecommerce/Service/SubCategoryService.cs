using Ecommerce.DTOs;
using Ecommerce.Entity;
using Ecommerce.Interfaces.Repositories;
using Ecommerce.Interfaces.Services;

namespace Ecommerce.Service;

public class SubCategoryService : ISubCategoryService
{
    private readonly ISubCategoryRepository _subCategoryRepository;
    private readonly ICategoryRepository _categoryRepository; // Verifica se a ctegoria pai existe

    public SubCategoryService(ISubCategoryRepository subCategoryRepository, ICategoryRepository categoryRepository)
    {
        _subCategoryRepository = subCategoryRepository;
        _categoryRepository = categoryRepository;
    }

    public object? CreateSubCategory(CreateSubCategoryDto subCategoryDto)
    {
        // === PRIMEIRA REGRA DE NEGÓCIO ===
        // Verificar se a Categoria Pai informada realmente existe no banco de dados.
        var parentCategory = _categoryRepository.GetById(subCategoryDto.ParentCategoryId);
        if (parentCategory == null)
        {
            // Se não existir, retornamos uma mensagem de erro em vez da entidade
            return new { Message = "A categoria pai informada não existe." };
        }

        var newSubCategory = new SubCategory
        {
            Name = subCategoryDto.Name,
            ParentCategoryId = subCategoryDto.ParentCategoryId
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
}