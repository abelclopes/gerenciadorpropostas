using System;
using System.Linq;
using System.Threading.Tasks;
using DOMAIN;
using DOMAIN.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace INFRAESTRUCTURE
{
    public class ApplicationDbContext : DbContext, IContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        :base(options)
        { }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<Proposta> Propostas { get; set; }
        public DbSet<PropostaAnexo> PropostaAnexos { get; set; }
        public DbSet<PropostaHistorico> PropostasHistoricos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Fornecedor>(entity =>
            {
                entity.HasIndex(e => e.CnpjCpf)
                    .IsUnique()
                    .HasFilter("([CnpjCpf] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Proposta>(entity =>
            {
                entity.HasIndex(e => e.CategoriaId);

                entity.HasIndex(e => e.FornecedorId);

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Descricao).HasMaxLength(500);

                entity.HasOne(d => d.Categoria)
                    .WithMany(p => p.Propostas)
                    .HasForeignKey(d => d.CategoriaId);

                entity.HasOne(d => d.Fornecedor)
                    .WithMany(p => p.Propostas)
                    .HasForeignKey(d => d.FornecedorId);
            });

            modelBuilder.Entity<PropostaHistorico>(entity =>
            {
                entity.HasIndex(e => e.PropostaId);

                entity.HasIndex(e => e.UsuarioId);

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Proposta)
                    .WithMany(p => p.PropostaHistorico)
                    .HasForeignKey(d => d.PropostaId);                    
            });

            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasIndex(e => e.Cpf)
                    .IsUnique()
                    .HasFilter("([Cpf] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedNever();
            });
            base.OnModelCreating(modelBuilder);
        }
        public async Task<int> SaveChangesAsync()
        {
            CheckUpdatedEntities();

            return await base.SaveChangesAsync();
        }

        public override int SaveChanges()
        {
            CheckUpdatedEntities();

            return base.SaveChanges();
        }


        private void CheckUpdatedEntities()
        {
            var updatedEntities = ChangeTracker.Entries().Where(x => x.State == EntityState.Modified);

            if (updatedEntities.Any())
            {
                var now = DateTime.UtcNow;

                updatedEntities.Select(x => x.Entity as EntidadeBase).ToList().ForEach(x => x.DataAtualizacao = now);
            }
        }
    }
}