namespace Ailos.ContaCorrente.Domain.Entities;

public class Transferencia
{
    public int Id { get; set; }
    public string IdRequisicao { get; set; } = string.Empty;
    public string ContaOrigem { get; set; } = string.Empty;
    public string ContaDestino { get; set; } = string.Empty;
    public decimal Valor { get; set; }
    public DateTime DataTransferencia { get; set; } = DateTime.UtcNow;
}
