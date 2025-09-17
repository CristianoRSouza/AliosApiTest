namespace Ailos.Transferencia.Application.DTOs;

public class TransferenciaDto
{
    public required string IdRequisicao { get; set; }
    public int ContaDestinoId { get; set; }
    public decimal Valor { get; set; }
}
