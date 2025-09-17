using Ailos.ContaCorrente.Application.Commands.CadastrarContaCorrente;
using Ailos.ContaCorrente.Domain.Exceptions;
using Ailos.ContaCorrente.Domain.Interfaces;
using Ailos.Shared.Common.ValueObjects;
using Ailos.Shared.Infrastructure.Security;
using FluentAssertions;
using Moq;
using Xunit;

namespace Ailos.ContaCorrente.UnitTests.Application.Commands;

public class CadastrarContaCorrenteHandlerTests
{
    private readonly Mock<IContaCorrenteRepository> _repositoryMock;
    private readonly Mock<PasswordHashService> _passwordServiceMock;
    private readonly CadastrarContaCorrenteHandler _handler;

    public CadastrarContaCorrenteHandlerTests()
    {
        _repositoryMock = new Mock<IContaCorrenteRepository>();
        _passwordServiceMock = new Mock<PasswordHashService>();
        _handler = new CadastrarContaCorrenteHandler(_repositoryMock.Object, _passwordServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateContaCorrenteAndReturnId()
    {
        // Arrange
        var command = new CadastrarContaCorrenteCommand
        {
            Cpf = "11144477735",
            Nome = "João Silva",
            Senha = "123456"
        };

        var hashedPassword = "hashedPassword123";
        var contaCriada = new Ailos.ContaCorrente.Domain.Entities.ContaCorrente
        {
            Id = 1,
            Cpf = new Cpf("11144477735"),
            Nome = new Nome("João Silva"),
            Senha = hashedPassword
        };

        _passwordServiceMock.Setup(x => x.HashPassword(command.Senha))
                           .Returns(hashedPassword);

        _repositoryMock.Setup(x => x.CreateAsync(It.IsAny<Ailos.ContaCorrente.Domain.Entities.ContaCorrente>()))
                      .ReturnsAsync(contaCriada);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(1);
        
        _passwordServiceMock.Verify(x => x.HashPassword(command.Senha), Times.Once);
        _repositoryMock.Verify(x => x.CreateAsync(It.Is<Ailos.ContaCorrente.Domain.Entities.ContaCorrente>(
            c => c.Cpf.Valor == command.Cpf && 
                 c.Nome.Valor == command.Nome && 
                 c.Senha == hashedPassword)), Times.Once);
    }

    [Theory]
    [InlineData("12345678901", "João Silva", "123456")] // CPF inválido
    [InlineData("11144477735", "", "123456")] // Nome vazio
    [InlineData("11144477735", "A", "123456")] // Nome muito curto
    public async Task Handle_InvalidValueObjects_ShouldThrowBusinessException(string cpf, string nome, string senha)
    {
        // Arrange
        var command = new CadastrarContaCorrenteCommand
        {
            Cpf = cpf,
            Nome = nome,
            Senha = senha
        };

        _passwordServiceMock.Setup(x => x.HashPassword(It.IsAny<string>()))
                           .Returns("hashedPassword");

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BusinessException>(() => 
            _handler.Handle(command, CancellationToken.None));
        
        exception.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_RepositoryThrowsException_ShouldPropagateException()
    {
        // Arrange
        var command = new CadastrarContaCorrenteCommand
        {
            Cpf = "11144477735",
            Nome = "João Silva",
            Senha = "123456"
        };

        _passwordServiceMock.Setup(x => x.HashPassword(It.IsAny<string>()))
                           .Returns("hashedPassword");

        _repositoryMock.Setup(x => x.CreateAsync(It.IsAny<Ailos.ContaCorrente.Domain.Entities.ContaCorrente>()))
                      .ThrowsAsync(new InvalidOperationException("Database error"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidOperationException>(() => 
            _handler.Handle(command, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldHashPassword()
    {
        // Arrange
        var command = new CadastrarContaCorrenteCommand
        {
            Cpf = "11144477735",
            Nome = "João Silva",
            Senha = "plainPassword"
        };

        var hashedPassword = "hashedPassword123";
        _passwordServiceMock.Setup(x => x.HashPassword("plainPassword"))
                           .Returns(hashedPassword);

        _repositoryMock.Setup(x => x.CreateAsync(It.IsAny<Ailos.ContaCorrente.Domain.Entities.ContaCorrente>()))
                      .ReturnsAsync(new Ailos.ContaCorrente.Domain.Entities.ContaCorrente { Id = 1 });

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _passwordServiceMock.Verify(x => x.HashPassword("plainPassword"), Times.Once);
        _repositoryMock.Verify(x => x.CreateAsync(It.Is<Ailos.ContaCorrente.Domain.Entities.ContaCorrente>(
            c => c.Senha == hashedPassword)), Times.Once);
    }
}
