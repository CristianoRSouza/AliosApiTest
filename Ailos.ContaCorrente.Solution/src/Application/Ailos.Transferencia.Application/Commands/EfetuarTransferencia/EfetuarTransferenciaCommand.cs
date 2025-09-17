using MediatR;

namespace Ailos.Transferencia.Application.Commands.EfetuarTransferencia;

public class EfetuarTransferenciaCommand : IRequest
{
    public required string IdRequisicao { get; set; }
    public int ContaOrigemId { get; set; }
    public int ContaDestinoId { get; set; }
    public decimal Valor { get; set; }
}
