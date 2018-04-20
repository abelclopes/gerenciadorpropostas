using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DOMAIN;

namespace INFRAESTRUCTURE.Data
{
     public class DbInitializer
    {
        public  void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            // Look for any students.
            if (context.Usuarios.Any())
            {
                return;   // DB has been seeded
            }
            
            
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



            context.SaveChanges();
        }
    }
}