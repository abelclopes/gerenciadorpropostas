using System;
using System.Linq;
using System.Threading.Tasks;
using DOMAIN;
using Microsoft.EntityFrameworkCore;

namespace INFRAESTRUCTURE
{
    public class ApplicationDbContext : DbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        :base(options)
        { }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Fornecedor> Fornecedores { get; set; }
        public DbSet<Proposta> Propostas { get; set; }
        public DbSet<PropostaHistorico> PropostasHistoricos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>()
                    .HasIndex(b => b.Cpf)
                    .IsUnique();

            modelBuilder.Entity<Fornecedor>()
                    .HasIndex(b => b.CnpjCpf)
                    .IsUnique();

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