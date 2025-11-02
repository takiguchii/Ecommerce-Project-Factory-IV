using Ecommerce.Dto;
using Ecommerce.Entity;
using Ecommerce.Interfaces.Repositories;
using Ecommerce.Service;
using FluentAssertions;
using Moq;
using Xunit;

namespace Ecommerce.Tests;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _productRepo = new();
    private readonly Mock<ICategoryRepository> _categoryRepo = new();
    private readonly Mock<ISubCategoryRepository> _subCategoryRepo = new();
    private readonly Mock<IBrandRepository> _brandRepo = new();
    private readonly Mock<IProviderRepository> _providerRepo = new();

    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _service = new ProductService(
            _productRepo.Object,
            _categoryRepo.Object,
            _subCategoryRepo.Object,
            _brandRepo.Object,
            _providerRepo.Object
        );
    }

    [Fact]
    public void CreateProduct_DeveRetornarNull_QuandoCategoriaNaoExistir()
    {
        // Arrange
        var dto = new CreateProductDto
        {
            name = "Mouse Gamer",
            code = "PROD001",
            category_id = 999,
            brand_id = 1,
            provider_id = 1
        };

        _categoryRepo.Setup(r => r.GetById(dto.category_id)).Returns((Category?)null);

        // Act
        var result = _service.CreateProduct(dto);

        // Assert
        result.Should().BeNull();
        _categoryRepo.Verify(r => r.GetById(dto.category_id), Times.Once);
        _productRepo.VerifyNoOtherCalls();
    }

    [Fact]
    public void CreateProduct_DeveCriarProduto_QuandoTudoExistir()
    {
        // Arrange
        var dto = new CreateProductDto
        {
            name = "Teclado Mecânico",
            code = "PROD002",
            category_id = 1,
            brand_id = 1,
            provider_id = 1,
            sub_category_id = 10
        };

        _categoryRepo.Setup(r => r.GetById(1)).Returns(new Category { id = 1, name = "Periféricos" });
        _brandRepo.Setup(r => r.GetById(1)).Returns(new Brand { id = 1, name = "HyperX" });
        _providerRepo.Setup(r => r.GetById(1)).Returns(new Provider { id = 1, name = "FornecedorX" });
        _subCategoryRepo.Setup(r => r.GetById(10)).Returns(new SubCategory { id = 10, name = "Teclados" });

        _productRepo.Setup(r => r.Add(It.IsAny<Product>()));
        _productRepo.Setup(r => r.SaveChanges());

        // Act
        var result = _service.CreateProduct(dto);

        // Assert
        result.Should().NotBeNull();
        result!.name.Should().Be("Teclado Mecânico");
        result.code.Should().Be("PROD002");

        _productRepo.Verify(r => r.Add(It.Is<Product>(p => p.name == "Teclado Mecânico" && p.code == "PROD002")), Times.Once);
        _productRepo.Verify(r => r.SaveChanges(), Times.Once);
    }

    [Fact]
    public void GetPromotions_DeveRetornarApenasProdutosComDesconto()
    {
        // Arrange
        var produtos = new List<Product>
        {
            new() { id = 1, name = "Produto 1", original_price = "100", discount_price = "80" },
            new() { id = 2, name = "Produto 2", original_price = null, discount_price = "50" },
            new() { id = 3, name = "Produto 3", original_price = "200", discount_price = "150" }
        };

        _productRepo.Setup(r => r.GetPromotions()).Returns(produtos);

        // Act
        var result = _service.GetPromotions();

        // Assert
        result.Should().HaveCount(2);
        result.All(p => p.discount_price is not null).Should().BeTrue();

        _productRepo.Verify(r => r.GetPromotions(), Times.Once);
        _productRepo.VerifyNoOtherCalls();
    }
}
