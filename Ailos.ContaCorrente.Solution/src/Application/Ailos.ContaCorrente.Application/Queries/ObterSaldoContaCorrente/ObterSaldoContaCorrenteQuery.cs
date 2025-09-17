using Ailos.ContaCorrente.Application.DTOs;
using MediatR;

namespace Ailos.ContaCorrente.Application.Queries.ObterSaldoContaCorrente;

public class ObterSaldoContaCorrenteQuery : IRequest<SaldoDto>
{
    public int ContaId { get; set; }
}
