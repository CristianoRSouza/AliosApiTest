using Ailos.Transferencia.Domain.Interfaces;
using Ailos.Transferencia.Domain.Exceptions;
using Ailos.Shared.Common.Enums;
using MediatR;

namespace Ailos.Transferencia.Application.Commands.EfetuarTransferencia;

public class EfetuarTransferenciaHandler : IRequestHandler<EfetuarTransferenciaCommand>
{
    private readonly ITransferenciaRepository _repository;

    public EfetuarTransferenciaHandler(ITransferenciaRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(EfetuarTransferenciaCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // 1. Validar valor positivo
            if (request.Valor <= 0)
                throw new BusinessException("Valor deve ser positivo", TipoFalha.INVALID_VALUE);

            // 2. Validar contas diferentes
            if (request.ContaOrigemId == request.ContaDestinoId)
                throw new BusinessException("Conta origem e destino devem ser diferentes", TipoFalha.INVALID_ACCOUNT);

            // 3. Persistir transferência
            var transferencia = new Domain.Entities.Transferencia
            {
                IdRequisicao = request.IdRequisicao,
                ContaOrigemId = request.ContaOrigemId,
                ContaDestinoId = request.ContaDestinoId,
                Valor = new Ailos.Shared.Common.ValueObjects.Dinheiro(request.Valor),
                DataTransferencia = DateTime.UtcNow
            };

            await _repository.CreateAsync(transferencia);
        }
        catch (Exception ex)
        {
            throw new BusinessException($"Erro na transferência: {ex.Message}", TipoFalha.INVALID_ACCOUNT);
        }
    }
}
