using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DOMAIN;
using DOMAIN.Interfaces;

namespace INFRAESTRUCTURE.Data
{
    public static class DbInitialize
    {
        public static void Seed(this IContext context)
        {
           // context.Database.EnsureCreated();
           
            // Look for any students.
            if (!context.Usuarios.Any())
            {
                var users = new Usuario[]
                {
                    // senha é teste123
                    new Usuario { Id = Guid.NewGuid(), Nome = "Administrador", Cpf = "9336423068", Email = "abellopes@gmail.com" , Senha = "2242461295221015719538209212227614317113501631961762", PerfilUsuario = Perfil.Administrador},
                    new Usuario { Id = Guid.NewGuid(), Nome = "Analista de comprar", Cpf = "99900299202", Email = "abell@gmail.com", Senha = "2242461295221015719538209212227614317113501631961762", PerfilUsuario = Perfil.AnalistaDeCompras },
                    new Usuario { Id = Guid.NewGuid(), Nome = "Analista Financeiro", Cpf = "98800299202", Email = "analista.financeiro@gmail.com", Senha = "2242461295221015719538209212227614317113501631961762", PerfilUsuario = Perfil.AnalistaFinanceiro },
                    new Usuario { Id = Guid.NewGuid(), Nome = "Diretor Financeiro", Cpf = "99977777722", Email = "Dfinanceiro@gmail.com", Senha = "2242461295221015719538209212227614317113501631961762", PerfilUsuario = Perfil.DiretorFinanceiro }
                };
                foreach (Usuario u in users)
                {
                    context.Usuarios.Add(u);
                }            
            }
            if (!context.Categorias.Any())
            {
            
                for(int i = 0; i < 5; i++)
                {
                    context.Categorias.Add(new Categoria { Id = Guid.NewGuid(), Nome = $"Categoria {i}", Descricao = $"Descricao {i}"});
                }
            }         
            
            if (!context.Fornecedores.Any())
            {
            
                for(int i = 0; i < 5; i++)
                {
                    context.Fornecedores.Add(new Fornecedor { Id = Guid.NewGuid(), Nome = $"Fornecedor {i}", CnpjCpf = gerador(), Email= $"fornecedor,{i}@email.com",Telefone=$"{gerador(9)}"});
                }
                
            }else{
                return;
            }            
            context.SaveChanges();
        }

        private static string gerador(int leng = 10)
        {
            Random R = new Random();
            return ((long)R.Next (0, 100000 ) * (long)R.Next (0, 100000 )).ToString ().PadLeft (leng, '0');
        }
    }
}