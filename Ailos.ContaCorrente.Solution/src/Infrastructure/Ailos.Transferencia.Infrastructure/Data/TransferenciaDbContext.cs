using Ailos.Shared.Common.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Ailos.Transferencia.Infrastructure.Data;

public class TransferenciaDbContext : DbContext
{
    public TransferenciaDbContext(DbContextOptions<TransferenciaDbContext> options) : base(options) { }

    public DbSet<Domain.Entities.Transferencia> Transferencias { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Domain.Entities.Transferencia>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.IdRequisicao).IsRequired();
            entity.Property(e => e.Valor)
                .HasConversion(
                    dinheiro => dinheiro.Valor, 
                    valor => new Dinheiro(valor))
                .HasColumnType("decimal(18,2)");
            entity.ToTable("TRANSFERENCIA");
        });
    }
}
