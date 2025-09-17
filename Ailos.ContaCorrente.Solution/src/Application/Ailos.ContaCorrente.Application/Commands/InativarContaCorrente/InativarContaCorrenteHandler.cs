using Ailos.ContaCorrente.Domain.Exceptions;
using Ailos.ContaCorrente.Domain.Interfaces;
using Ailos.Shared.Common.Enums;
using Ailos.Shared.Infrastructure.Security;
using MediatR;

namespace Ailos.ContaCorrente.Application.Commands.InativarContaCorrente;

public class InativarContaCorrenteHandler : IRequestHandler<InativarContaCorrenteCommand>
{
    private readonly IContaCorrenteRepository _repository;
    private readonly PasswordHashService _passwordHashService;

    public InativarContaCorrenteHandler(IContaCorrenteRepository repository, PasswordHashService passwordHashService)
    {
        _repository = repository;
        _passwordHashService = passwordHashService;
    }

    public async Task Handle(InativarContaCorrenteCommand request, CancellationToken cancellationToken)
    {
        var conta = await _repository.GetByIdAsync(request.ContaId);
        if (conta == null)
            throw new BusinessException("Conta corrente não encontrada", TipoFalha.INVALID_ACCOUNT);

        if (!_passwordHashService.VerifyPassword(request.Senha, conta.Senha))
            throw new BusinessException("Senha incorreta", TipoFalha.USER_UNAUTHORIZED);

        conta.Ativo = false;
        await _repository.UpdateAsync(conta);
    }
}
