using Ailos.ContaCorrente.Domain.Entities;
using Ailos.ContaCorrente.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Ailos.ContaCorrente.Infrastructure.Data.Repositories;

public class MovimentoRepository : IMovimentoRepository
{
    private readonly AppDbContext _context;

    public MovimentoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Movimento> CreateAsync(Movimento movimento)
    {
        _context.Movimentos.Add(movimento);
        await _context.SaveChangesAsync();
        return movimento;
    }

    public async Task<List<Movimento>> GetByContaIdAsync(int contaId)
    {
        return await _context.Movimentos
            .Where(m => m.ContaCorrenteId == contaId)
            .ToListAsync();
    }
}
