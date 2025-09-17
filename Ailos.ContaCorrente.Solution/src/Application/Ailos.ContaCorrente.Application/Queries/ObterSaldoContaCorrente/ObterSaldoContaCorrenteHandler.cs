using Ailos.ContaCorrente.Application.DTOs;
using Ailos.ContaCorrente.Domain.Enums;
using Ailos.ContaCorrente.Domain.Exceptions;
using Ailos.ContaCorrente.Domain.Interfaces;
using Ailos.Shared.Common.Enums;
using MediatR;

namespace Ailos.ContaCorrente.Application.Queries.ObterSaldoContaCorrente;

public class ObterSaldoContaCorrenteHandler : IRequestHandler<ObterSaldoContaCorrenteQuery, SaldoDto>
{
    private readonly IContaCorrenteRepository _contaRepository;

    public ObterSaldoContaCorrenteHandler(IContaCorrenteRepository contaRepository)
    {
        _contaRepository = contaRepository;
    }

    public async Task<SaldoDto> Handle(ObterSaldoContaCorrenteQuery request, CancellationToken cancellationToken)
    {
        var conta = await _contaRepository.GetByIdAsync(request.ContaId);
        if (conta == null)
            throw new BusinessException("Conta corrente não encontrada", TipoFalha.INVALID_ACCOUNT);

        if (!conta.Ativo)
            throw new BusinessException("Conta corrente inativa", TipoFalha.INACTIVE_ACCOUNT);

        var saldo = conta.CalcularSaldo();

        return new SaldoDto
        {
            NumeroConta = conta.Id,
            NomeTitular = conta.Nome.Valor,
            DataHoraConsulta = DateTime.UtcNow,
            Saldo = saldo.Valor
        };
    }
}
