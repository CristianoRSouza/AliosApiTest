namespace Ailos.ContaCorrente.Application.DTOs;

public class CadastrarContaRequest
{
    public string Cpf { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public string Senha { get; set; } = string.Empty;
}

public class LoginRequest
{
    public string? NumeroConta { get; set; }
    public string? Cpf { get; set; }
    public string Senha { get; set; } = string.Empty;
}

public class InativarContaRequest
{
    public string Senha { get; set; } = string.Empty;
}

public class MovimentacaoRequest
{
    public string IdRequisicao { get; set; } = string.Empty;
    public string? NumeroConta { get; set; }
    public decimal Valor { get; set; }
    public char TipoMovimento { get; set; }
}

public class SaldoResponse
{
    public string NumeroConta { get; set; } = string.Empty;
    public string NomeTitular { get; set; } = string.Empty;
    public DateTime DataConsulta { get; set; }
    public decimal Saldo { get; set; }
}

public class TransferenciaRequest
{
    public string IdRequisicao { get; set; } = string.Empty;
    public string ContaDestino { get; set; } = string.Empty;
    public decimal Valor { get; set; }
}
