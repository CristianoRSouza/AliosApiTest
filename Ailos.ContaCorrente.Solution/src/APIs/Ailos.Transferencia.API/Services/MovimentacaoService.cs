using System.Text;
using System.Text.Json;

namespace Ailos.Transferencia.API.Services;

public class MovimentacaoService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<MovimentacaoService> _logger;

    public MovimentacaoService(IHttpClientFactory httpClientFactory, ILogger<MovimentacaoService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<bool> MovimentarAsync(string idRequisicao, decimal valor, string tipo, int? contaId = null, string? authToken = null)
    {
        try
        {
            var httpClient = _httpClientFactory.CreateClient("ContaCorrenteAPI");
            
            if (!string.IsNullOrEmpty(authToken))
            {
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Add("Authorization", authToken);
            }

            var request = new
            {
                IdRequisicao = idRequisicao,
                ContaCorrenteId = contaId,
                Valor = valor,
                Tipo = tipo
            };

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            _logger.LogInformation("Enviando movimentação para: {BaseUrl}/api/contacorrente/movimentacao", httpClient.BaseAddress);

            var response = await httpClient.PostAsync("/api/contacorrente/movimentacao", content);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Erro na movimentação: {StatusCode} - {Content}", response.StatusCode, errorContent);
            }

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao chamar API de movimentação");
            return false;
        }
    }
}
