using Ailos.ContaCorrente.Domain.Enums;
using Ailos.Shared.Common.ValueObjects;

namespace Ailos.ContaCorrente.Domain.Entities;

public class Movimento
{
    public int Id { get; set; }
    public required string IdRequisicao { get; set; }
    public int ContaCorrenteId { get; set; }
    public required Dinheiro Valor { get; set; }
    public TipoMovimento Tipo { get; set; }
    public DateTime DataMovimento { get; set; } = DateTime.UtcNow;
    public ContaCorrente? ContaCorrente { get; set; }
}
