using MediatR;

namespace Ailos.ContaCorrente.Application.Commands.MovimentarContaCorrente;

public class MovimentarContaCorrenteCommand : IRequest
{
    public required string IdRequisicao { get; set; }
    public int? ContaCorrenteId { get; set; }
    public decimal Valor { get; set; }
    public required string Tipo { get; set; }
    public int ContaLogadaId { get; set; }
}
