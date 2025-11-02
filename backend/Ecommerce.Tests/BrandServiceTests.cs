using Ecommerce.DTOs;
using Ecommerce.Entity;
using Ecommerce.Interfaces.Repositories;
using Ecommerce.Service;
using FluentAssertions;
using Moq;
using Xunit;

namespace Ecommerce.Tests;

public class BrandServiceTests
{
    private readonly Mock<IBrandRepository> _repo = new();
    private readonly BrandService _service;

    public BrandServiceTests()
    {
        _service = new BrandService(_repo.Object);
    }

    [Fact]
    public void GetAll_DeveRetornarLista()
    {
        // Arrange
        var data = new List<Brand>
        {
            new() { id = 1, name = "Logitech", brand_image_url = "img1" },
            new() { id = 2, name = "Razer",    brand_image_url = "img2" }
        };
        _repo.Setup(r => r.GetAll()).Returns(data);

        // Act
        var result = _service.GetAll();

        // Assert
        result.Should().HaveCount(2).And.Contain(b => b.name == "Logitech");
        _repo.Verify(r => r.GetAll(), Times.Once);
        _repo.VerifyNoOtherCalls();
    }

    [Fact]
    public void CreateBrand_DeveCriarSalvarERetornarMarca()
    {
        // Arrange
        var dto = new CreateBrandDto { name = "HyperX", brand_image_url = "logo.png" };
        _repo.Setup(r => r.Add(It.IsAny<Brand>()));
        _repo.Setup(r => r.SaveChanges());

        // Act
        var result = _service.CreateBrand(dto);

        // Assert
        result.name.Should().Be("HyperX");
        result.brand_image_url.Should().Be("logo.png");

        _repo.Verify(r => r.Add(It.Is<Brand>(b => b.name == "HyperX" && b.brand_image_url == "logo.png")), Times.Once);
        _repo.Verify(r => r.SaveChanges(), Times.Once);
        _repo.VerifyNoOtherCalls();
    }

    [Fact]
    public void DeleteBrand_DeveRetornarFalse_QuandoNaoEncontrar()
    {
        // Arrange
        _repo.Setup(r => r.GetById(99)).Returns((Brand?)null);

        // Act
        var ok = _service.DeleteBrand(99);

        // Assert
        ok.Should().BeFalse();
        _repo.Verify(r => r.GetById(99), Times.Once);
        _repo.VerifyNoOtherCalls();
    }

    [Fact]
    public void DeleteBrand_DeveDeletar_QuandoEncontrar()
    {
        // Arrange
        var entity = new Brand { id = 3, name = "Corsair", brand_image_url = "c.png" };
        _repo.Setup(r => r.GetById(3)).Returns(entity);
        _repo.Setup(r => r.Delete(entity));
        _repo.Setup(r => r.SaveChanges());

        // Act
        var ok = _service.DeleteBrand(3);

        // Assert
        ok.Should().BeTrue();
        _repo.Verify(r => r.GetById(3), Times.Once);
        _repo.Verify(r => r.Delete(It.Is<Brand>(b => b.id == 3)), Times.Once);
        _repo.Verify(r => r.SaveChanges(), Times.Once);
        _repo.VerifyNoOtherCalls();
    }

    [Fact]
    public void UpdateBrand_DeveRetornarNull_QuandoNaoEncontrar()
    {
        // Arrange
        _repo.Setup(r => r.GetById(1000)).Returns((Brand?)null);
        var dto = new CreateBrandDto { name = "Novo", brand_image_url = "n.png" };

        // Act
        var updated = _service.UpdateBrand(1000, dto);

        // Assert
        updated.Should().BeNull();
        _repo.Verify(r => r.GetById(1000), Times.Once);
        _repo.VerifyNoOtherCalls();
    }

    [Fact]
    public void UpdateBrand_DeveAtualizar_QuandoEncontrar()
    {
        // Arrange
        var existing = new Brand { id = 4, name = "Antiga", brand_image_url = "a.png" };
        _repo.Setup(r => r.GetById(4)).Returns(existing);
        _repo.Setup(r => r.Update(existing));
        _repo.Setup(r => r.SaveChanges());

        var dto = new CreateBrandDto { name = "Atualizada", brand_image_url = "nova.png" };

        // Act
        var updated = _service.UpdateBrand(4, dto)!;

        // Assert
        updated.id.Should().Be(4);
        updated.name.Should().Be("Atualizada");
        updated.brand_image_url.Should().Be("nova.png");

        _repo.Verify(r => r.GetById(4), Times.Once);
        _repo.Verify(r => r.Update(It.Is<Brand>(b => b.id == 4 && b.name == "Atualizada" && b.brand_image_url == "nova.png")), Times.Once);
        _repo.Verify(r => r.SaveChanges(), Times.Once);
        _repo.VerifyNoOtherCalls();
    }
}
