using Ecommerce.DTOs;
using Ecommerce.Entity;
using Ecommerce.Interfaces.Repositories;
using Ecommerce.Service;
using FluentAssertions;
using Moq;
using Xunit;

namespace Ecommerce.Tests;

public class ProviderServiceTests
{
    private readonly Mock<IProviderRepository> _repo = new();
    private readonly ProviderService _service;

    public ProviderServiceTests()
    {
        _service = new ProviderService(_repo.Object);
    }

    [Fact]
    public void CreateProvider_DeveRetornarMensagem_QuandoCnpjJaExistir()
    {
        // Arrange
        var dto = new CreateProviderDto
        {
            name = "Fornecedor Teste",
            cnpj = "12345678000199",
            email = "teste@email.com",
            phone_number = "11999999999",
            address = "Rua X, 123"
        };

        _repo.Setup(r => r.GetByCnpj(dto.cnpj))
             .Returns(new Provider { id = 1, name = "Existente", cnpj = dto.cnpj });

        // Act
        var result = _service.CreateProvider(dto);

        // Assert
        var message = result?.GetType().GetProperty("Message")?.GetValue(result)?.ToString();
        message.Should().Be("Já existe um fornecedor cadastrado com este CNPJ.");

        _repo.Verify(r => r.GetByCnpj(dto.cnpj), Times.Once);
        _repo.VerifyNoOtherCalls();
    }

    [Fact]
    public void CreateProvider_DeveCriarNovoFornecedor_QuandoCnpjNaoExistir()
    {
        // Arrange
        var dto = new CreateProviderDto
        {
            name = "Fornecedor Novo",
            cnpj = "99988877000155",
            email = "novo@email.com",
            phone_number = "11988887777",
            address = "Rua Nova, 456"
        };

        _repo.Setup(r => r.GetByCnpj(dto.cnpj)).Returns((Provider?)null);
        _repo.Setup(r => r.Add(It.IsAny<Provider>()));
        _repo.Setup(r => r.SaveChanges());

        // Act
        var result = _service.CreateProvider(dto);

        // Assert
        result.Should().BeOfType<Provider>();
        var provider = (Provider)result!;
        provider.name.Should().Be("Fornecedor Novo");
        provider.cnpj.Should().Be("99988877000155");

        _repo.Verify(r => r.GetByCnpj(dto.cnpj), Times.Once);
        _repo.Verify(r => r.Add(It.Is<Provider>(p => p.name == "Fornecedor Novo" && p.cnpj == "99988877000155")), Times.Once);
        _repo.Verify(r => r.SaveChanges(), Times.Once);
        _repo.VerifyNoOtherCalls();
    }

    [Fact]
    public void DeleteProvider_DeveRetornarFalse_QuandoNaoEncontrar()
    {
        // Arrange
        _repo.Setup(r => r.GetById(99)).Returns((Provider?)null);

        // Act
        var result = _service.DeleteProvider(99);

        // Assert
        result.Should().BeFalse();
        _repo.Verify(r => r.GetById(99), Times.Once);
        _repo.VerifyNoOtherCalls();
    }

    [Fact]
    public void DeleteProvider_DeveExcluir_QuandoEncontrar()
    {
        // Arrange
        var entity = new Provider { id = 5, name = "Fornecedor X", cnpj = "12345678000199" };
        _repo.Setup(r => r.GetById(5)).Returns(entity);
        _repo.Setup(r => r.Delete(entity));
        _repo.Setup(r => r.SaveChanges());

        // Act
        var result = _service.DeleteProvider(5);

        // Assert
        result.Should().BeTrue();
        _repo.Verify(r => r.GetById(5), Times.Once);
        _repo.Verify(r => r.Delete(It.Is<Provider>(p => p.id == 5)), Times.Once);
        _repo.Verify(r => r.SaveChanges(), Times.Once);
        _repo.VerifyNoOtherCalls();
    }

    [Fact]
    public void UpdateProvider_DeveRetornarNull_QuandoNaoEncontrar()
    {
        // Arrange
        _repo.Setup(r => r.GetById(999)).Returns((Provider?)null);
        var dto = new CreateProviderDto
        {
            name = "Atualizado",
            cnpj = "00011122000133",
            email = "novo@teste.com",
            phone_number = "11911112222",
            address = "Rua Atualizada, 789"
        };

        // Act
        var result = _service.UpdateProvider(999, dto);

        // Assert
        result.Should().BeNull();
        _repo.Verify(r => r.GetById(999), Times.Once);
        _repo.VerifyNoOtherCalls();
    }

    [Fact]
    public void UpdateProvider_DeveAtualizarDados_QuandoEncontrar()
    {
        // Arrange
        var existing = new Provider
        {
            id = 2,
            name = "Antigo",
            cnpj = "11122233000144",
            email = "antigo@email.com",
            phone_number = "11922223333",
            address = "Rua Velha"
        };

        var dto = new CreateProviderDto
        {
            name = "Novo Nome",
            cnpj = "11122233000144",
            email = "novo@email.com",
            phone_number = "11944445555",
            address = "Rua Nova"
        };

        _repo.Setup(r => r.GetById(2)).Returns(existing);
        _repo.Setup(r => r.Update(existing));
        _repo.Setup(r => r.SaveChanges());

        // Act
        var result = _service.UpdateProvider(2, dto)!;

        // Assert
        result.id.Should().Be(2);
        result.name.Should().Be("Novo Nome");
        result.email.Should().Be("novo@email.com");
        result.phone_number.Should().Be("11944445555");
        result.address.Should().Be("Rua Nova");

        _repo.Verify(r => r.GetById(2), Times.Once);
        _repo.Verify(r => r.Update(It.Is<Provider>(p => p.id == 2 && p.name == "Novo Nome")), Times.Once);
        _repo.Verify(r => r.SaveChanges(), Times.Once);
        _repo.VerifyNoOtherCalls();
    }
}
