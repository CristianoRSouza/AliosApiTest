using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Xunit;

namespace Ailos.IntegrationTests;

public class ContaCorrenteApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public ContaCorrenteApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task CadastrarContaCorrente_ValidData_ShouldReturnSuccess()
    {
        // Arrange
        var request = new
        {
            cpf = "11144477735",
            nome = "João Silva",
            senha = "123456"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/contacorrente/cadastrar", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<JsonElement>(content);
        result.GetProperty("numeroConta").GetInt32().Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task Login_ValidCredentials_ShouldReturnToken()
    {
        // Arrange - Primeiro cadastrar uma conta
        var cadastroRequest = new
        {
            cpf = "98765432100",
            nome = "Maria Santos",
            senha = "654321"
        };
        
        var cadastroResponse = await _client.PostAsJsonAsync("/api/contacorrente/cadastrar", cadastroRequest);
        var cadastroContent = await cadastroResponse.Content.ReadAsStringAsync();
        var cadastroResult = JsonSerializer.Deserialize<JsonElement>(cadastroContent);
        var numeroConta = cadastroResult.GetProperty("numeroConta").GetInt32();

        var loginRequest = new
        {
            contaOuCpf = numeroConta.ToString(),
            senha = "654321"
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/contacorrente/login", loginRequest);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<JsonElement>(content);
        result.GetProperty("token").GetString().Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Saldo_WithoutToken_ShouldReturnUnauthorized()
    {
        // Act
        var response = await _client.GetAsync("/api/contacorrente/saldo");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
