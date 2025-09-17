using Ailos.Transferencia.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ailos.Transferencia.Infrastructure.Data.Repositories;

public class TransferenciaRepository : ITransferenciaRepository
{
    private readonly TransferenciaDbContext _context;

    public TransferenciaRepository(TransferenciaDbContext context)
    {
        _context = context;
    }

    public async Task<Domain.Entities.Transferencia> CreateAsync(Domain.Entities.Transferencia transferencia)
    {
        _context.Transferencias.Add(transferencia);
        await _context.SaveChangesAsync();
        return transferencia;
    }
}
