using Ailos.ContaCorrente.Application.Commands.CadastrarContaCorrente;
using Ailos.ContaCorrente.Application.Commands.InativarContaCorrente;
using Ailos.ContaCorrente.Application.Commands.MovimentarContaCorrente;
using Ailos.ContaCorrente.Application.DTOs;
using Ailos.ContaCorrente.Application.Queries.ObterSaldoContaCorrente;
using Ailos.ContaCorrente.Application.Queries.ValidarLogin;
using Ailos.ContaCorrente.Domain.Exceptions;
using Ailos.Shared.Common.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ailos.ContaCorrente.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContaCorrenteController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ICacheService _cache;
    private readonly IEventPublisher _eventPublisher;

    public ContaCorrenteController(IMediator mediator, ICacheService cache, IEventPublisher eventPublisher)
    {
        _mediator = mediator;
        _cache = cache;
        _eventPublisher = eventPublisher;
    }

    private int GetCurrentUserId()
    {
        var contaIdClaim = User.FindFirst("contaId")?.Value;
        return int.TryParse(contaIdClaim, out var contaId) ? contaId : 0;
    }

    [HttpPost("cadastrar")]
    public async Task<IActionResult> Cadastrar([FromBody] CadastrarContaCorrenteDto dto)
    {
        try
        {
            var command = new CadastrarContaCorrenteCommand
            {
                Cpf = dto.Cpf,
                Senha = dto.Senha,
                Nome = dto.Nome
            };

            var numeroConta = await _mediator.Send(command);

            // Publicar evento no Kafka
            await _eventPublisher.PublishAsync("conta-criada", new
            {
                NumeroConta = numeroConta,
                Cpf = dto.Cpf,
                Nome = dto.Nome,
                DataCriacao = DateTime.UtcNow
            });

            return Ok(new { NumeroConta = numeroConta });
        }
        catch (BusinessException ex)
        {
            return BadRequest(new { Mensagem = ex.Message, Tipo = ex.TipoFalha.ToString() });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            var query = new ValidarLoginQuery
            {
                ContaOuCpf = dto.ContaOuCpf,
                Senha = dto.Senha
            };

            var token = await _mediator.Send(query);

            // Publicar evento de login no Kafka
            await _eventPublisher.PublishAsync("usuario-logado", new
            {
                ContaOuCpf = dto.ContaOuCpf,
                DataLogin = DateTime.UtcNow
            });

            return Ok(new { Token = token });
        }
        catch (BusinessException ex)
        {
            return Unauthorized(new { Mensagem = ex.Message, Tipo = ex.TipoFalha.ToString() });
        }
    }

    [HttpPost("inativar")]
    [Authorize]
    public async Task<IActionResult> Inativar([FromBody] InativarContaCorrenteDto dto)
    {
        try
        {
            var contaId = GetCurrentUserId();
            if (contaId == 0) return Unauthorized("Token inválido");

            var command = new InativarContaCorrenteCommand
            {
                Senha = dto.Senha,
                ContaId = contaId
            };

            await _mediator.Send(command);

            // Remover do cache
            await _cache.RemoveAsync($"saldo-{contaId}");

            // Publicar evento no Kafka
            await _eventPublisher.PublishAsync("conta-inativada", new
            {
                ContaId = contaId,
                DataInativacao = DateTime.UtcNow
            });

            return NoContent();
        }
        catch (BusinessException ex)
        {
            return BadRequest(new { Mensagem = ex.Message, Tipo = ex.TipoFalha.ToString() });
        }
    }

    [HttpPost("movimentacao")]
    [Authorize]
    public async Task<IActionResult> Movimentacao([FromBody] MovimentacaoDto dto)
    {
        try
        {
            var contaId = GetCurrentUserId();
            if (contaId == 0) return Unauthorized("Token inválido");

            var command = new MovimentarContaCorrenteCommand
            {
                IdRequisicao = dto.IdRequisicao,
                ContaCorrenteId = dto.ContaCorrenteId,
                Valor = dto.Valor,
                Tipo = dto.Tipo,
                ContaLogadaId = contaId
            };

            await _mediator.Send(command);

            // Invalidar cache do saldo
            var contaAfetada = dto.ContaCorrenteId ?? contaId;
            await _cache.RemoveAsync($"saldo-{contaAfetada}");
            if (contaAfetada != contaId)
            {
                await _cache.RemoveAsync($"saldo-{contaId}");
            }

            // Publicar evento no Kafka
            await _eventPublisher.PublishAsync("movimentacao-realizada", new
            {
                IdRequisicao = dto.IdRequisicao,
                ContaId = contaAfetada,
                Valor = dto.Valor,
                Tipo = dto.Tipo,
                DataMovimentacao = DateTime.UtcNow
            });

            return NoContent();
        }
        catch (BusinessException ex)
        {
            return BadRequest(new { Mensagem = ex.Message, Tipo = ex.TipoFalha.ToString() });
        }
    }

    [HttpGet("saldo")]
    [Authorize]
    public async Task<IActionResult> Saldo()
    {
        try
        {
            var contaId = GetCurrentUserId();
            if (contaId == 0) return Unauthorized("Token inválido");

            // Tentar buscar no cache primeiro
            var cacheKey = $"saldo-{contaId}";
            var saldoCache = await _cache.GetAsync<SaldoDto>(cacheKey);
            
            if (saldoCache != null)
            {
                return Ok(saldoCache);
            }

            // Se não estiver no cache, buscar no banco
            var query = new ObterSaldoContaCorrenteQuery { ContaId = contaId };
            var saldo = await _mediator.Send(query);

            // Salvar no cache por 5 minutos
            await _cache.SetAsync(cacheKey, saldo, TimeSpan.FromMinutes(5));

            return Ok(saldo);
        }
        catch (BusinessException ex)
        {
            return BadRequest(new { Mensagem = ex.Message, Tipo = ex.TipoFalha.ToString() });
        }
    }
}
