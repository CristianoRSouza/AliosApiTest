namespace Ailos.ContaCorrente.Domain.Interfaces;

public interface IMovimentoRepository
{
    Task<Entities.Movimento> CreateAsync(Entities.Movimento movimento);
    Task<List<Entities.Movimento>> GetByContaIdAsync(int contaId);
}
