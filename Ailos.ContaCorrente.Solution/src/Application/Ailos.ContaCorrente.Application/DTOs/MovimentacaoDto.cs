namespace Ailos.ContaCorrente.Application.DTOs;

public class MovimentacaoDto
{
    public required string IdRequisicao { get; set; }
    public int? ContaCorrenteId { get; set; }
    public decimal Valor { get; set; }
    public required string Tipo { get; set; }
}
