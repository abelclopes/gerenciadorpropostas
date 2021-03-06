using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace DOMAIN.Interfaces
{
    public interface IContext
    {
        DbSet<Usuario> Usuarios { get; set; }
        DbSet<Permissao> Permissoes { get; set; }
        DbSet<UsuarioPermissao> UsuarioPermissoes { get; set; }
        DbSet<Fornecedor> Fornecedores { get; set; }
        DbSet<Proposta> Propostas { get; set; }
        DbSet<PropostaAnexo> PropostaAnexos { get; set; }
        DbSet<PropostaHistorico> PropostasHistoricos { get; set; }
        DbSet<Categoria> Categorias { get; set; }        
        DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync();
        int SaveChanges();
    }
}