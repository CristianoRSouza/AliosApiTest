namespace Ailos.ContaCorrente.Application.DTOs;

public class CadastrarContaCorrenteDto
{
    public required string Cpf { get; set; }
    public required string Senha { get; set; }
    public required string Nome { get; set; }
}
