namespace Ailos.Shared.Common.ValueObjects;

public record Cpf
{
    public string Valor { get; }

    public Cpf(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor) || valor.Length != 11)
            throw new ArgumentException("CPF deve ter 11 dígitos", nameof(valor));
        
        Valor = valor;
    }

    public static implicit operator string(Cpf cpf) => cpf.Valor;
    public static implicit operator Cpf(string valor) => new(valor);
    
    public override string ToString() => Valor;
}
