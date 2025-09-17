using Ailos.Shared.Common.ValueObjects;

namespace Ailos.Transferencia.Domain.Entities;

public class Transferencia
{
    public int Id { get; set; }
    public string IdRequisicao { get; set; } = string.Empty;
    public int ContaOrigemId { get; set; }
    public int ContaDestinoId { get; set; }
    public Dinheiro Valor { get; set; } = new(0);
    public DateTime DataTransferencia { get; set; } = DateTime.UtcNow;
}
