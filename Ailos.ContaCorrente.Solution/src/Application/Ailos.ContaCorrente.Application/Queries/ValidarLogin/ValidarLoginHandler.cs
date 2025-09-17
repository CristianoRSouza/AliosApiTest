using Ailos.ContaCorrente.Domain.Exceptions;
using Ailos.ContaCorrente.Domain.Interfaces;
using Ailos.ContaCorrente.Application.Services;
using Ailos.Shared.Common.ValueObjects;
using Ailos.Shared.Common.Enums;
using Ailos.Shared.Infrastructure.Security;
using MediatR;

namespace Ailos.ContaCorrente.Application.Queries.ValidarLogin;

public class ValidarLoginHandler : IRequestHandler<ValidarLoginQuery, string>
{
    private readonly IContaCorrenteRepository _repository;
    private readonly IJwtService _jwtService;
    private readonly PasswordHashService _passwordHashService;

    public ValidarLoginHandler(IContaCorrenteRepository repository, IJwtService jwtService, PasswordHashService passwordHashService)
    {
        _repository = repository;
        _jwtService = jwtService;
        _passwordHashService = passwordHashService;
    }

    public async Task<string> Handle(ValidarLoginQuery request, CancellationToken cancellationToken)
    {
        Domain.Entities.ContaCorrente? conta = null;

        if (int.TryParse(request.ContaOuCpf, out int contaId))
        {
            conta = await _repository.GetByIdAsync(contaId);
        }
        else
        {
            try
            {
                var cpf = new Cpf(request.ContaOuCpf);
                conta = await _repository.GetByCpfAsync(cpf);
            }
            catch (ArgumentException)
            {
                throw new BusinessException("CPF inválido", TipoFalha.INVALID_DOCUMENT);
            }
        }

        if (conta == null || !conta.Ativo)
            throw new BusinessException("Conta não encontrada ou inativa", TipoFalha.INACTIVE_ACCOUNT);

        if (!_passwordHashService.VerifyPassword(request.Senha, conta.Senha))
            throw new BusinessException("Usuário não autorizado", TipoFalha.USER_UNAUTHORIZED);

        return _jwtService.GenerateToken(conta.Id.ToString());
    }
}
