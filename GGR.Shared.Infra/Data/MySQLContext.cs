using GGR.Shared.Infra.Model;
using Microsoft.EntityFrameworkCore;

namespace GGR.Shared.Infra.Data
{
    public class MySQLContext : DbContext
    {
        public MySQLContext(DbContextOptions<MySQLContext> options) 
            : base(options) { }


        public DbSet<Pessoa>? Pessoas { get; set; }

        public DbSet<Categoria>? Categorias { get; set; }

        public DbSet<Transacao>? Transacoes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pessoa>(entity =>
            {
                entity.ToTable("Pessoas");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                      .ValueGeneratedNever();

                entity.Property(x => x.Nome)
                      .HasMaxLength(150)
                      .IsRequired(true);

                entity.Property(x => x.Idade)
                      .IsRequired(true);

                entity.Property(x => x.DataCriacaoRegistro)
                      .IsRequired(true);
            });

            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.ToTable("Categorias");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                      .ValueGeneratedNever();

                entity.Property(x => x.Descricao)
                      .HasMaxLength(150)
                      .IsRequired(false);

                entity.Property(x => x.Finalidade)
                      .IsRequired()
                      .HasConversion<int>();

                entity.Property(x => x.DataCriacaoRegistro)
                      .IsRequired(true);
            });

            modelBuilder.Entity<Transacao>(entity =>
            {
                entity.ToTable("Transacoes");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Valor)
                      .HasColumnType("decimal(18,2)")
                      .IsRequired();

                entity.HasOne(x => x.Categoria)
                      .WithMany()
                      .HasForeignKey(x => x.CategoriaId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Pessoa)
                      .WithMany()
                      .HasForeignKey(x => x.PessoaId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.Property(x => x.DataCriacaoRegistro)
                      .IsRequired(true);

                entity.HasOne(t => t.Pessoa)
                      .WithMany(p => p.Transacoes)
                      .HasForeignKey(t => t.PessoaId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
