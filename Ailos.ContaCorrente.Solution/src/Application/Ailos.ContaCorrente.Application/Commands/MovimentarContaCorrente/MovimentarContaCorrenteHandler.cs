using Ailos.ContaCorrente.Domain.Enums;
using Ailos.ContaCorrente.Domain.Exceptions;
using Ailos.ContaCorrente.Domain.Interfaces;
using Ailos.Shared.Common.ValueObjects;
using Ailos.Shared.Common.Enums;
using MediatR;

namespace Ailos.ContaCorrente.Application.Commands.MovimentarContaCorrente;

public class MovimentarContaCorrenteHandler : IRequestHandler<MovimentarContaCorrenteCommand>
{
    private readonly IContaCorrenteRepository _contaRepository;
    private readonly IMovimentoRepository _movimentoRepository;

    public MovimentarContaCorrenteHandler(IContaCorrenteRepository contaRepository, IMovimentoRepository movimentoRepository)
    {
        _contaRepository = contaRepository;
        _movimentoRepository = movimentoRepository;
    }

    public async Task Handle(MovimentarContaCorrenteCommand request, CancellationToken cancellationToken)
    {
        // Usar conta do token se não informada
        var contaId = request.ContaCorrenteId ?? request.ContaLogadaId;
        
        var conta = await _contaRepository.GetByIdAsync(contaId);
        if (conta == null)
            throw new BusinessException("Conta corrente não encontrada", TipoFalha.INVALID_ACCOUNT);

        if (!conta.Ativo)
            throw new BusinessException("Conta corrente inativa", TipoFalha.INACTIVE_ACCOUNT);

        if (request.Valor <= 0)
            throw new BusinessException("Valor deve ser positivo", TipoFalha.INVALID_VALUE);

        var tipoMovimento = request.Tipo.ToUpper() switch
        {
            "C" => TipoMovimento.Credito,
            "D" => TipoMovimento.Debito,
            _ => throw new BusinessException("Tipo de movimento inválido", TipoFalha.INVALID_TYPE)
        };

        // Validar se é crédito em conta diferente
        if (contaId != request.ContaLogadaId && tipoMovimento != TipoMovimento.Credito)
            throw new BusinessException("Apenas crédito permitido em conta diferente", TipoFalha.INVALID_TYPE);

        var movimento = new Domain.Entities.Movimento
        {
            IdRequisicao = request.IdRequisicao,
            ContaCorrenteId = contaId,
            Valor = new Dinheiro(request.Valor),
            Tipo = tipoMovimento
        };

        await _movimentoRepository.CreateAsync(movimento);
    }
}
