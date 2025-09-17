namespace Ailos.Shared.Common.ValueObjects;

public record Nome
{
    public string Valor { get; }

    public Nome(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("Nome é obrigatório", nameof(valor));
        
        if (valor.Length < 2)
            throw new ArgumentException("Nome deve ter pelo menos 2 caracteres", nameof(valor));
            
        if (valor.Length > 100)
            throw new ArgumentException("Nome não pode ter mais de 100 caracteres", nameof(valor));
        
        Valor = valor.Trim();
    }

    public static implicit operator string(Nome nome) => nome.Valor;
    public static implicit operator Nome(string valor) => new(valor);
    
    public override string ToString() => Valor;
}
