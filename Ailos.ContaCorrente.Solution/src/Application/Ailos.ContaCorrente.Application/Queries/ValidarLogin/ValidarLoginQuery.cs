using MediatR;

namespace Ailos.ContaCorrente.Application.Queries.ValidarLogin;

public class ValidarLoginQuery : IRequest<string>
{
    public required string ContaOuCpf { get; set; }
    public required string Senha { get; set; }
}
