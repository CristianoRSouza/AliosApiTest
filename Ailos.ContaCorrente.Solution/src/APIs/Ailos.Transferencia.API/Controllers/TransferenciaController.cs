using Ailos.Transferencia.API.Services;
using Ailos.Transferencia.Application.Commands.EfetuarTransferencia;
using Ailos.Transferencia.Application.DTOs;
using Ailos.Transferencia.Domain.Exceptions;
using Confluent.Kafka;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace Ailos.Transferencia.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransferenciaController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly MovimentacaoService _movimentacaoService;
    private readonly IProducer<string, string> _kafkaProducer;
    private readonly ILogger<TransferenciaController> _logger;

    public TransferenciaController(IMediator mediator, MovimentacaoService movimentacaoService, IProducer<string, string> kafkaProducer, ILogger<TransferenciaController> logger)
    {
        _mediator = mediator;
        _movimentacaoService = movimentacaoService;
        _kafkaProducer = kafkaProducer;
        _logger = logger;
    }

    private int GetCurrentUserId()
    {
        var contaIdClaim = User.FindFirst("contaId")?.Value;
        return int.TryParse(contaIdClaim, out var contaId) ? contaId : 0;
    }

    private async Task PublishKafkaEvent<T>(string topic, string key, T eventData)
    {
        try
        {
            var json = JsonSerializer.Serialize(eventData);
            var message = new Message<string, string> { Key = key, Value = json };
            await _kafkaProducer.ProduceAsync(topic, message);
            _logger.LogInformation("Evento publicado no Kafka: {Topic} - {Key}", topic, key);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao publicar evento no Kafka: {Topic} - {Key}", topic, key);
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> EfetuarTransferencia([FromBody] TransferenciaDto dto)
    {
        try
        {
            var contaId = GetCurrentUserId();
            if (contaId == 0) return Unauthorized("Token inválido");

            var authToken = Request.Headers["Authorization"].ToString();

            // 1. Débito na conta origem
            var debitoSucesso = await _movimentacaoService.MovimentarAsync(
                dto.IdRequisicao + "_DEBITO", 
                dto.Valor, 
                "D", 
                null, 
                authToken
            );

            if (!debitoSucesso)
            {
                await PublishKafkaEvent("transferencia-falhou", dto.IdRequisicao, new
                {
                    IdRequisicao = dto.IdRequisicao,
                    ContaOrigemId = contaId,
                    ContaDestinoId = dto.ContaDestinoId,
                    Valor = dto.Valor,
                    Motivo = "Falha no débito",
                    DataFalha = DateTime.UtcNow
                });

                return BadRequest(new { Mensagem = "Falha no débito - saldo insuficiente ou conta inválida", Tipo = "INVALID_ACCOUNT" });
            }

            // 2. Crédito na conta destino
            var creditoSucesso = await _movimentacaoService.MovimentarAsync(
                dto.IdRequisicao + "_CREDITO", 
                dto.Valor, 
                "C", 
                dto.ContaDestinoId,
                authToken
            );

            if (!creditoSucesso)
            {
                // Estorno
                await _movimentacaoService.MovimentarAsync(
                    dto.IdRequisicao + "_ESTORNO", 
                    dto.Valor, 
                    "C", 
                    null,
                    authToken
                );

                await PublishKafkaEvent("transferencia-falhou", dto.IdRequisicao, new
                {
                    IdRequisicao = dto.IdRequisicao,
                    ContaOrigemId = contaId,
                    ContaDestinoId = dto.ContaDestinoId,
                    Valor = dto.Valor,
                    Motivo = "Falha no crédito - estorno realizado",
                    DataFalha = DateTime.UtcNow
                });
                
                return BadRequest(new { Mensagem = "Falha no crédito - conta destino inválida", Tipo = "INVALID_ACCOUNT" });
            }

            // 3. Registrar transferência
            var command = new EfetuarTransferenciaCommand
            {
                IdRequisicao = dto.IdRequisicao,
                ContaDestinoId = dto.ContaDestinoId,
                Valor = dto.Valor,
                ContaOrigemId = contaId
            };

            await _mediator.Send(command);

            // 4. Publicar evento de sucesso no Kafka
            await PublishKafkaEvent("transferencia-realizada", dto.IdRequisicao, new
            {
                IdRequisicao = dto.IdRequisicao,
                ContaOrigemId = contaId,
                ContaDestinoId = dto.ContaDestinoId,
                Valor = dto.Valor,
                DataTransferencia = DateTime.UtcNow
            });

            return NoContent();
        }
        catch (BusinessException ex)
        {
            await PublishKafkaEvent("transferencia-erro", dto.IdRequisicao, new
            {
                IdRequisicao = dto.IdRequisicao,
                Erro = ex.Message,
                DataErro = DateTime.UtcNow
            });

            return BadRequest(new { Mensagem = ex.Message, Tipo = ex.TipoFalha.ToString() });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado na transferência: {IdRequisicao}", dto.IdRequisicao);
            return BadRequest(new { Mensagem = ex.Message });
        }
    }

}
