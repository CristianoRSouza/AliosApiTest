using Ailos.ContaCorrente.Domain.Entities;
using Ailos.Shared.Common.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Ailos.ContaCorrente.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Domain.Entities.ContaCorrente> ContasCorrentes { get; set; }
    public DbSet<Movimento> Movimentos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Domain.Entities.ContaCorrente>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Cpf)
                .HasConversion(
                    cpf => cpf.Valor, 
                    valor => new Cpf(valor))
                .HasColumnName("Cpf")
                .IsRequired();
            entity.Property(e => e.Nome)
                .HasConversion(
                    nome => nome.Valor, 
                    valor => new Nome(valor))
                .HasColumnName("Nome")
                .IsRequired();
            entity.Property(e => e.Senha).IsRequired();
            entity.ToTable("CONTACORRENTE");
        });

        modelBuilder.Entity<Movimento>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Valor)
                .HasConversion(
                    dinheiro => dinheiro.Valor, 
                    valor => new Dinheiro(valor))
                .HasColumnType("decimal(18,2)");
            entity.Property(e => e.Tipo)
                .HasConversion<string>();
            entity.ToTable("MOVIMENTO");
        });
    }
}
