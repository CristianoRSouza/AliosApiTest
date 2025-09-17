using Ailos.ContaCorrente.Domain.Exceptions;
using Ailos.ContaCorrente.Domain.Interfaces;
using Ailos.Shared.Common.ValueObjects;
using Ailos.Shared.Common.Enums;
using Ailos.Shared.Infrastructure.Security;
using MediatR;

namespace Ailos.ContaCorrente.Application.Commands.CadastrarContaCorrente;

public class CadastrarContaCorrenteHandler : IRequestHandler<CadastrarContaCorrenteCommand, int>
{
    private readonly IContaCorrenteRepository _repository;
    private readonly PasswordHashService _passwordHashService;

    public CadastrarContaCorrenteHandler(IContaCorrenteRepository repository, PasswordHashService passwordHashService)
    {
        _repository = repository;
        _passwordHashService = passwordHashService;
    }

    public async Task<int> Handle(CadastrarContaCorrenteCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var cpf = new Cpf(request.Cpf);
            var nome = new Nome(request.Nome);

            // Verificar se CPF já existe
            var contaExistente = await _repository.GetByCpfAsync(cpf);
            if (contaExistente != null)
                throw new BusinessException("CPF já cadastrado", TipoFalha.INVALID_DOCUMENT);

            var senhaHash = _passwordHashService.HashPassword(request.Senha);
            var conta = new Domain.Entities.ContaCorrente(cpf, nome, senhaHash);

            var result = await _repository.CreateAsync(conta);
            return result.Id;
        }
        catch (ArgumentException ex)
        {
            throw new BusinessException(ex.Message, TipoFalha.INVALID_DOCUMENT);
        }
    }
}
