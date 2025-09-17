using Ailos.Shared.Common.ValueObjects;

namespace Ailos.ContaCorrente.Domain.Interfaces;

public interface IContaCorrenteRepository
{
    Task<Entities.ContaCorrente?> GetByIdAsync(int id);
    Task<Entities.ContaCorrente?> GetByCpfAsync(Cpf cpf);
    Task<Entities.ContaCorrente> CreateAsync(Entities.ContaCorrente conta);
    Task UpdateAsync(Entities.ContaCorrente conta);
    Task<bool> ExistsAsync(int id);
}
