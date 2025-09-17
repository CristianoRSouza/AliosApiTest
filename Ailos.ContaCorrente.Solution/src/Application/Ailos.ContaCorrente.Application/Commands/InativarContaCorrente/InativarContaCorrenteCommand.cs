using MediatR;

namespace Ailos.ContaCorrente.Application.Commands.InativarContaCorrente;

public class InativarContaCorrenteCommand : IRequest
{
    public required string Senha { get; set; }
    public int ContaId { get; set; }
}
