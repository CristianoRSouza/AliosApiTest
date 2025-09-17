using Ailos.ContaCorrente.Application.Commands.CadastrarContaCorrente;
using Ailos.ContaCorrente.Application.Validators;
using FluentAssertions;
using Xunit;

namespace Ailos.ContaCorrente.UnitTests.Application.Validators;

public class CadastrarContaCorrenteValidatorTests
{
    private readonly CadastrarContaCorrenteValidator _validator;

    public CadastrarContaCorrenteValidatorTests()
    {
        _validator = new CadastrarContaCorrenteValidator();
    }

    [Theory]
    [InlineData("11144477735", "João Silva", "123456", true)]
    [InlineData("98765432100", "Maria Santos", "password123", true)]
    public async Task Validate_ValidCommand_ShouldBeValid(string cpf, string nome, string senha, bool expectedValid)
    {
        // Arrange
        var command = new CadastrarContaCorrenteCommand
        {
            Cpf = cpf,
            Nome = nome,
            Senha = senha
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().Be(expectedValid);
    }

    [Theory]
    [InlineData("", "João Silva", "123456", "CPF é obrigatório")]
    [InlineData("123456789", "João Silva", "123456", "CPF deve ter 11 dígitos")]
    [InlineData("123456789012", "João Silva", "123456", "CPF deve ter 11 dígitos")]
    [InlineData("1234567890a", "João Silva", "123456", "CPF deve conter apenas números")]
    public async Task Validate_InvalidCpf_ShouldHaveValidationError(string cpf, string nome, string senha, string expectedError)
    {
        // Arrange
        var command = new CadastrarContaCorrenteCommand
        {
            Cpf = cpf,
            Nome = nome,
            Senha = senha
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains(expectedError));
    }

    [Theory]
    [InlineData("11144477735", "", "123456", "Nome é obrigatório")]
    [InlineData("11144477735", "A", "123456", "Nome deve ter pelo menos 2 caracteres")]
    [InlineData("11144477735", new string('A', 101), "123456", "Nome não pode ter mais de 100 caracteres")]
    public async Task Validate_InvalidNome_ShouldHaveValidationError(string cpf, string nome, string senha, string expectedError)
    {
        // Arrange
        var command = new CadastrarContaCorrenteCommand
        {
            Cpf = cpf,
            Nome = nome,
            Senha = senha
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains(expectedError));
    }

    [Theory]
    [InlineData("11144477735", "João Silva", "", "Senha é obrigatória")]
    [InlineData("11144477735", "João Silva", "12345", "Senha deve ter pelo menos 6 caracteres")]
    public async Task Validate_InvalidSenha_ShouldHaveValidationError(string cpf, string nome, string senha, string expectedError)
    {
        // Arrange
        var command = new CadastrarContaCorrenteCommand
        {
            Cpf = cpf,
            Nome = nome,
            Senha = senha
        };

        // Act
        var result = await _validator.ValidateAsync(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(e => e.ErrorMessage.Contains(expectedError));
    }
}
