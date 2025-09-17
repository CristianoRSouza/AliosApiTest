namespace Ailos.Shared.Common.ValueObjects;

public record Dinheiro
{
    public decimal Valor { get; }

    public Dinheiro(decimal valor)
    {
        if (valor < 0)
            throw new ArgumentException("Valor não pode ser negativo", nameof(valor));
        
        Valor = Math.Round(valor, 2);
    }

    public static implicit operator decimal(Dinheiro dinheiro) => dinheiro.Valor;
    public static implicit operator Dinheiro(decimal valor) => new(valor);
    
    public static Dinheiro operator +(Dinheiro a, Dinheiro b) => new(a.Valor + b.Valor);
    public static Dinheiro operator -(Dinheiro a, Dinheiro b) => new(a.Valor - b.Valor);
    
    public override string ToString() => Valor.ToString("C");
}
