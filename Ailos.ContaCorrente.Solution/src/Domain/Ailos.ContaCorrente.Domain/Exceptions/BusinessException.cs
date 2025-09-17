using Ailos.Shared.Common.Enums;

namespace Ailos.ContaCorrente.Domain.Exceptions;

public class BusinessException : Exception
{
    public TipoFalha TipoFalha { get; }

    public BusinessException(string message, TipoFalha tipoFalha) : base(message)
    {
        TipoFalha = tipoFalha;
    }
}
