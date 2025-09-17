using Ailos.ContaCorrente.Domain.Interfaces;
using Ailos.Shared.Common.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Ailos.ContaCorrente.Infrastructure.Data.Repositories;

public class ContaCorrenteRepository : IContaCorrenteRepository
{
    private readonly AppDbContext _context;

    public ContaCorrenteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Domain.Entities.ContaCorrente> CreateAsync(Domain.Entities.ContaCorrente conta)
    {
        _context.ContasCorrentes.Add(conta);
        await _context.SaveChangesAsync();
        return conta;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.ContasCorrentes.AnyAsync(c => c.Id == id);
    }

    public async Task<Domain.Entities.ContaCorrente?> GetByCpfAsync(Cpf cpf)
    {
        return await _context.ContasCorrentes
            .Where(c => c.Cpf == cpf)
            .FirstOrDefaultAsync();
    }

    public async Task<Domain.Entities.ContaCorrente?> GetByIdAsync(int id)
    {
        return await _context.ContasCorrentes
            .Include(c => c.Movimentos)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task UpdateAsync(Domain.Entities.ContaCorrente conta)
    {
        _context.ContasCorrentes.Update(conta);
        await _context.SaveChangesAsync();
    }
}
