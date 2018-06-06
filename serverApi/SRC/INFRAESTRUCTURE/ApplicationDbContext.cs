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
        public DbSet<Permissao> Permissoes { get; set; }
        public DbSet<UsuarioPermissao> UsuarioPermissoes { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<Proposta> Propostas { get; set; }
        public DbSet<PropostaAnexo> PropostaAnexos { get; set; }
        public DbSet<PropostaHistorico> PropostasHistoricos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { 
            modelBuilder.Entity<Categoria>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Fornecedor>(entity =>
            {
                entity.HasIndex(e => e.CnpjCpf)
                    .IsUnique()
                    .HasFilter("([CnpjCpf] IS NOT NULL)");

                entity.Property(e => e.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Proposta>(entity =>
            {
                entity.HasIndex(e => e.CategoriaId);

                entity.HasIndex(e => e.FornecedorId);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Descricao).HasMaxLength(500);

                // entity.HasOne(d => d.Categoria)
                //     .WithMany(p => p.Propostas)
                //     .HasForeignKey(d => d.CategoriaId);

                // entity.HasOne(d => d.Fornecedor)
                //     .WithMany(p => p.Propostas)
                //     .HasForeignKey(d => d.FornecedorId);
            });

            modelBuilder.Entity<PropostaHistorico>(entity =>
            {
                entity.HasIndex(e => e.PropostaId);

                entity.HasIndex(e => e.UsuarioId);

                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Propostas)
                    .WithMany(p => p.PropostaHistoricos)
                    .HasForeignKey(d => d.PropostaId);                    
            });


            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.Cpf).IsRequired().HasMaxLength(18);

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.Nome).IsRequired();

                entity.Property(e => e.Senha).IsRequired();
            });

            modelBuilder.Entity<UsuarioPermissao>(entity =>
            {
                entity.HasKey(e => e.UsuarioId);

                entity.Property(e => e.UsuarioId).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Permissoes)
                    .WithMany(p => p.UsuarioPermissoes)
                    .HasForeignKey(d => d.PermissaoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsuarioPermissoes_Permissoes");

                entity.HasOne(d => d.Usuario)
                    .WithOne(p => p.UsuarioPermissoes)
                    .HasForeignKey<UsuarioPermissao>(d => d.UsuarioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UsuarioPermissoes_Usuarios");
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