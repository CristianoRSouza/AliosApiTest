namespace Ailos.Transferencia.Domain.Interfaces;

public interface ITransferenciaRepository
{
    Task<Entities.Transferencia> CreateAsync(Entities.Transferencia transferencia);
}
