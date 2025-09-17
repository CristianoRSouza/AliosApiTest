using Ailos.Shared.Common.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Ailos.ContaCorrente.UnitTests.Domain.ValueObjects;

public class DinheiroTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(10.50)]
    [InlineData(1000.99)]
    [InlineData(0.01)]
    public void Dinheiro_ValidValue_ShouldCreateSuccessfully(decimal value)
    {
        // Act
        var dinheiro = new Dinheiro(value);

        // Assert
        dinheiro.Valor.Should().Be(Math.Round(value, 2));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-10.50)]
    [InlineData(-0.01)]
    public void Dinheiro_NegativeValue_ShouldThrowArgumentException(decimal negativeValue)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Dinheiro(negativeValue));
        exception.Message.Should().Contain("Valor não pode ser negativo");
    }

    [Fact]
    public void Dinheiro_Addition_ShouldWorkCorrectly()
    {
        // Arrange
        var dinheiro1 = new Dinheiro(10.50m);
        var dinheiro2 = new Dinheiro(5.25m);

        // Act
        var result = dinheiro1 + dinheiro2;

        // Assert
        result.Valor.Should().Be(15.75m);
    }

    [Fact]
    public void Dinheiro_Subtraction_ShouldWorkCorrectly()
    {
        // Arrange
        var dinheiro1 = new Dinheiro(10.50m);
        var dinheiro2 = new Dinheiro(5.25m);

        // Act
        var result = dinheiro1 - dinheiro2;

        // Assert
        result.Valor.Should().Be(5.25m);
    }

    [Fact]
    public void Dinheiro_ImplicitConversionToDecimal_ShouldReturnValue()
    {
        // Arrange
        var dinheiro = new Dinheiro(10.50m);

        // Act
        decimal value = dinheiro;

        // Assert
        value.Should().Be(10.50m);
    }

    [Fact]
    public void Dinheiro_ImplicitConversionFromDecimal_ShouldCreateDinheiro()
    {
        // Arrange
        decimal value = 10.50m;

        // Act
        Dinheiro dinheiro = value;

        // Assert
        dinheiro.Valor.Should().Be(10.50m);
    }

    [Fact]
    public void Dinheiro_ToString_ShouldReturnFormattedCurrency()
    {
        // Arrange
        var dinheiro = new Dinheiro(10.50m);

        // Act
        var result = dinheiro.ToString();

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain("10");
        result.Should().Contain("50");
    }

    [Fact]
    public void Dinheiro_RoundingPrecision_ShouldRoundToTwoDecimals()
    {
        // Arrange & Act
        var dinheiro = new Dinheiro(10.999m);

        // Assert
        dinheiro.Valor.Should().Be(11.00m);
    }
}
