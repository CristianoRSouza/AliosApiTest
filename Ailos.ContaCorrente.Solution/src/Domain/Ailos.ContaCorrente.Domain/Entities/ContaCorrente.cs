using Ailos.Shared.Common.ValueObjects;
using Ailos.ContaCorrente.Domain.Enums;

namespace Ailos.ContaCorrente.Domain.Entities;

public class ContaCorrente
{
    public int Id { get; set; }
    public Cpf Cpf { get; set; } = null!;
    public string Senha { get; set; } = null!;
    public Nome Nome { get; set; } = null!;
    public bool Ativo { get; set; } = true;
    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
    public List<Movimento> Movimentos { get; set; } = new();

    public ContaCorrente() { }

    public ContaCorrente(Cpf cpf, Nome nome, string senha)
    {
        Cpf = cpf;
        Nome = nome;
        Senha = senha;
        Ativo = true;
        DataCriacao = DateTime.UtcNow;
        Movimentos = new List<Movimento>();
    }
    
    public Dinheiro CalcularSaldo()
    {
        var creditos = Movimentos.Where(m => m.Tipo == TipoMovimento.Credito).Sum(m => m.Valor.Valor);
        var debitos = Movimentos.Where(m => m.Tipo == TipoMovimento.Debito).Sum(m => m.Valor.Valor);
        return new Dinheiro(creditos - debitos);
    }
}
