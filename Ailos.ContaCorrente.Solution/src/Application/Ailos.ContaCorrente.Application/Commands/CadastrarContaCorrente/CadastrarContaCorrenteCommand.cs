using MediatR;

namespace Ailos.ContaCorrente.Application.Commands.CadastrarContaCorrente;

public class CadastrarContaCorrenteCommand : IRequest<int>
{
    public required string Cpf { get; set; }
    public required string Senha { get; set; }
    public required string Nome { get; set; }
}
