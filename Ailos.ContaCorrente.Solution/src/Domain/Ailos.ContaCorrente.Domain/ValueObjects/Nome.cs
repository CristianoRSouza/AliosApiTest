namespace Ailos.ContaCorrente.Domain.ValueObjects;

public record Nome
{
    public string Valor { get; }

    public Nome(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("Nome não pode ser vazio", nameof(valor));
        
        if (valor.Length < 2 || valor.Length > 200)
            throw new ArgumentException("Nome deve ter entre 2 e 200 caracteres", nameof(valor));
        
        Valor = valor.Trim();
    }

    public static implicit operator string(Nome nome) => nome.Valor;
    public static implicit operator Nome(string valor) => new(valor);
    
    public override string ToString() => Valor;
}
