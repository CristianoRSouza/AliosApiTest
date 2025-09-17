using Ailos.Transferencia.Application.Commands.EfetuarTransferencia;
using Ailos.Transferencia.Domain.Interfaces;
using Ailos.Transferencia.Domain.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace Ailos.Transferencia.UnitTests.Application.Commands;

public class EfetuarTransferenciaHandlerTests
{
    private readonly Mock<ITransferenciaRepository> _repositoryMock;
    private readonly EfetuarTransferenciaHandler _handler;

    public EfetuarTransferenciaHandlerTests()
    {
        _repositoryMock = new Mock<ITransferenciaRepository>();
        _handler = new EfetuarTransferenciaHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateTransferencia()
    {
        // Arrange
        var command = new EfetuarTransferenciaCommand
        {
            IdRequisicao = "TRANS-001",
            ContaOrigemId = 1,
            ContaDestinoId = 2,
            Valor = 100.50m
        };

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(x => x.CreateAsync(It.IsAny<Domain.Entities.Transferencia>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ValorZero_ShouldThrowBusinessException()
    {
        // Arrange
        var command = new EfetuarTransferenciaCommand
        {
            IdRequisicao = "TRANS-001",
            ContaOrigemId = 1,
            ContaDestinoId = 2,
            Valor = 0
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BusinessException>(() => _handler.Handle(command, CancellationToken.None));
        exception.Message.Should().Contain("Valor deve ser positivo");
    }

    [Fact]
    public async Task Handle_ContasIguais_ShouldThrowBusinessException()
    {
        // Arrange
        var command = new EfetuarTransferenciaCommand
        {
            IdRequisicao = "TRANS-001",
            ContaOrigemId = 1,
            ContaDestinoId = 1,
            Valor = 100.50m
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BusinessException>(() => _handler.Handle(command, CancellationToken.None));
        exception.Message.Should().Contain("Conta origem e destino devem ser diferentes");
    }
}
