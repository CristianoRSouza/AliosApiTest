namespace Ailos.ContaCorrente.Application.DTOs;

public class SaldoDto
{
    public int NumeroConta { get; set; }
    public required string NomeTitular { get; set; }
    public DateTime DataHoraConsulta { get; set; }
    public decimal Saldo { get; set; }
}
