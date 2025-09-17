namespace Ailos.ContaCorrente.Application.DTOs;

public class LoginDto
{
    public required string ContaOuCpf { get; set; }
    public required string Senha { get; set; }
}
