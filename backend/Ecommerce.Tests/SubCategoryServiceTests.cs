using Ecommerce.DTOs;
using Ecommerce.Entity;
using Ecommerce.Interfaces.Repositories;
using Ecommerce.Service;
using FluentAssertions;
using Moq;
using Xunit;

namespace Ecommerce.Tests;

public class SubCategoryServiceTests
{
    private readonly Mock<ISubCategoryRepository> _subRepo = new();
    private readonly Mock<ICategoryRepository> _catRepo = new();
    private readonly SubCategoryService _service;

    public SubCategoryServiceTests()
    {
        _service = new SubCategoryService(_subRepo.Object, _catRepo.Object);
    }

    [Fact]
    public void DeveRetornarMensagem_QuandoCategoriaPaiNaoExiste()
    {
        // Arrange
        var dto = new CreateSubCategoryDto { name = "Teclados", category_id = 999 };
        _catRepo.Setup(r => r.GetById(dto.category_id)).Returns((Category?)null);

        // Act
        var result = _service.CreateSubCategory(dto);

        // Assert
        var message = result?.GetType().GetProperty("Message")?.GetValue(result)?.ToString();
        message.Should().Be("A categoria pai informada não existe.");

        _catRepo.Verify(r => r.GetById(dto.category_id), Times.Once);
        _subRepo.VerifyNoOtherCalls();
    }

    [Fact]
    public void DeveCriarSubCategoria_QuandoCategoriaPaiExiste()
    {
        // Arrange
        var dto = new CreateSubCategoryDto { name = "Mouses", category_id = 1 };
        var parent = new Category { id = 1, name = "Periféricos" };

        _catRepo.Setup(r => r.GetById(1)).Returns(parent);
        _subRepo.Setup(r => r.Add(It.IsAny<SubCategory>()));
        _subRepo.Setup(r => r.SaveChanges());

        // Act
        var result = _service.CreateSubCategory(dto);

        // Assert
        result.Should().BeOfType<SubCategory>();
        var sub = (SubCategory)result!;
        sub.name.Should().Be("Mouses");
        sub.category_id.Should().Be(1);

        _catRepo.Verify(r => r.GetById(1), Times.Once);
        _subRepo.Verify(r => r.Add(It.Is<SubCategory>(s => s.name == "Mouses" && s.category_id == 1)), Times.Once);
        _subRepo.Verify(r => r.SaveChanges(), Times.Once);
    }
}
