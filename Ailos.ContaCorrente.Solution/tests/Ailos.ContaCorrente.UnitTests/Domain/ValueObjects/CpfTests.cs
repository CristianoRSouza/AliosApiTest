using Ailos.Shared.Common.ValueObjects;
using FluentAssertions;
using Xunit;

namespace Ailos.ContaCorrente.UnitTests.Domain.ValueObjects;

public class CpfTests
{
    [Theory]
    [InlineData("11144477735")]
    [InlineData("98765432100")]
    [InlineData("12345678901")]
    public void Cpf_ValidFormat_ShouldCreateSuccessfully(string cpfValue)
    {
        // Act
        var cpf = new Cpf(cpfValue);

        // Assert
        cpf.Valor.Should().Be(cpfValue);
    }

    [Theory]
    [InlineData("123456789", "CPF deve ter 11 dígitos")]
    [InlineData("123456789012", "CPF deve ter 11 dígitos")]
    [InlineData("", "CPF deve ter 11 dígitos")]
    [InlineData("   ", "CPF deve ter 11 dígitos")]
    [InlineData(null, "CPF deve ter 11 dígitos")]
    [InlineData("1234567890a", "CPF deve ter 11 dígitos")]
    public void Cpf_InvalidFormat_ShouldThrowArgumentException(string invalidCpf, string expectedMessage)
    {
        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => new Cpf(invalidCpf));
        exception.Message.Should().Contain(expectedMessage);
    }

    [Fact]
    public void Cpf_ImplicitConversionToString_ShouldReturnValue()
    {
        // Arrange
        var cpf = new Cpf("11144477735");

        // Act
        string cpfString = cpf;

        // Assert
        cpfString.Should().Be("11144477735");
    }

    [Fact]
    public void Cpf_ImplicitConversionFromString_ShouldCreateCpf()
    {
        // Arrange
        string cpfValue = "11144477735";

        // Act
        Cpf cpf = cpfValue;

        // Assert
        cpf.Valor.Should().Be(cpfValue);
    }

    [Fact]
    public void Cpf_ToString_ShouldReturnValue()
    {
        // Arrange
        var cpf = new Cpf("11144477735");

        // Act
        var result = cpf.ToString();

        // Assert
        result.Should().Be("11144477735");
    }

    [Fact]
    public void Cpf_EqualityComparison_ShouldWorkCorrectly()
    {
        // Arrange
        var cpf1 = new Cpf("11144477735");
        var cpf2 = new Cpf("11144477735");
        var cpf3 = new Cpf("98765432100");

        // Act & Assert
        cpf1.Should().Be(cpf2);
        cpf1.Should().NotBe(cpf3);
        (cpf1 == cpf2).Should().BeTrue();
        (cpf1 == cpf3).Should().BeFalse();
    }
}
